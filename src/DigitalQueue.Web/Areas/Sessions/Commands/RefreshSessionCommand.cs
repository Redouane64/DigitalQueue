using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Infrastructure;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Sessions.Commands;

public class RefreshSessionCommand : IRequest<TokenResult?>
{
    public string RefreshToken { get; }

    public RefreshSessionCommand(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
    
    class RefreshSessionCommandHandler : IRequestHandler<RefreshSessionCommand, TokenResult?>
    {
        private readonly JwtTokenService _jwtTokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DigitalQueueContext _context;
        private readonly ILogger<RefreshSessionCommandHandler> _logger;

        public RefreshSessionCommandHandler(
            JwtTokenService jwtTokenService,
            IHttpContextAccessor httpContextAccessor,
            DigitalQueueContext context,
            ILogger<RefreshSessionCommandHandler> logger)
        {
            _jwtTokenService = jwtTokenService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _logger = logger;
        }
        
        public async Task<TokenResult?> Handle(RefreshSessionCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            var session = await _context.Sessions
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.RefreshToken.Equals(request.RefreshToken), cancellationToken);

            if (session is null)
            {
                _logger.LogWarning("Session does not exist");
                return null;
            }
            
            var tokens = await _jwtTokenService.RefreshToken(request.RefreshToken, session.User,
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, session.User.Id),
                    new Claim(ClaimTypesDefaults.Session, session.Id)
                });

            if (tokens is null)
            {
                return null;
            }
            
            var deviceToken = _httpContextAccessor.HttpContext!.Request.Headers["X-Device-Token"].ToString();
            var deviceIp = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].ToString() 
                           ?? _httpContextAccessor.HttpContext!.Connection.RemoteIpAddress!.ToString();

            session.AccessToken = tokens.AccessToken;
            session.RefreshToken = tokens.RefreshToken;
            session.DeviceToken = deviceToken;
            session.DeviceIP = deviceIp;

            _context.Update(session);

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return tokens;
        }
    }
}
