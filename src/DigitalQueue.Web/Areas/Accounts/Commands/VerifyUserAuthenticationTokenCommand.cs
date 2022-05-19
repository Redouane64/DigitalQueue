using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Sessions.Commands;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class VerifyUserAuthenticationTokenCommand : IRequest<AuthenticationResultDto?>
{
    public string Email { get; }
    public string Token { get; }

    public VerifyUserAuthenticationTokenCommand(string email, string token)
    {
        Email = email;
        Token = token;
    }
    
    public class VerifyUserAuthenticationTokenCommandHandler : IRequestHandler<VerifyUserAuthenticationTokenCommand, AuthenticationResultDto?>
    {
        private readonly DigitalQueueUserManager _userManager;
        private readonly DigitalQueueContext _context;
        private readonly JwtTokenService _jwtTokenService;
        private readonly IMediator _mediator;
        private readonly ILogger<VerifyUserAuthenticationTokenCommandHandler> _logger;

        public VerifyUserAuthenticationTokenCommandHandler(
            DigitalQueueUserManager userManager, 
            DigitalQueueContext context, 
            JwtTokenService jwtTokenService, 
            IMediator mediator,
            ILogger<VerifyUserAuthenticationTokenCommandHandler> logger)
        {
            _userManager = userManager;
            _context = context;
            _jwtTokenService = jwtTokenService;
            _mediator = mediator;
            _logger = logger;
        }
        
        public async Task<AuthenticationResultDto?> Handle(VerifyUserAuthenticationTokenCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                await transaction.RollbackAsync(cancellationToken);
                
                _logger.LogWarning("User {Email} does not exist", request.Email);
                return null;
            }

            var validToken = await _userManager.VerifyUserTokenAsync(user, AuthenticationTokenProvider.ProviderName,
                AuthenticationTokenProvider.AuthenticationPurposeName, request.Token);

            if (!validToken)
            {
                await transaction.RollbackAsync(cancellationToken);

                _logger.LogWarning("User token is not valid");
                return null;
            }
            
            var sessionSecurityStamp = new Claim(ClaimTypesDefaults.Session, Guid.NewGuid().ToString());
            var userClaims = await _userManager.GetClaimsAsync(user);
            var claims = userClaims.Union(new [] { sessionSecurityStamp });
            var tokenResult = await _jwtTokenService.GenerateToken(claims, user);
            
            await _mediator.Send(new CreateSessionCommand(user.Id, sessionSecurityStamp.Value,
                tokenResult.AccessToken, tokenResult.RefreshToken), cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return new AuthenticationResultDto(
                sessionSecurityStamp.Value, // <- FYI: field is ignored
                tokenResult.AccessToken, 
                tokenResult.RefreshToken);
        }
    }
}
