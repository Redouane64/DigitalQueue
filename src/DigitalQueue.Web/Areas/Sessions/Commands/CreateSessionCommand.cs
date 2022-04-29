using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using MediatR;

namespace DigitalQueue.Web.Areas.Sessions.Commands;

public class CreateSessionCommand : IRequest
{
    public string UserId { get; }
    public string SessionId { get; }
    public string AccessToken { get; }
    public string RefreshToken { get; }

    public CreateSessionCommand(string userId, string sessionId, string accessToken, string refreshToken)
    {
        UserId = userId;
        SessionId = sessionId;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand>
    {
        private readonly DigitalQueueContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateSessionCommandHandler(DigitalQueueContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<Unit> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
        {
            var deviceToken = _httpContextAccessor.HttpContext!.Request.Headers["X-Device-Token"].ToString();
            var deviceIp = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].ToString() 
                           ?? _httpContextAccessor.HttpContext!.Connection.RemoteIpAddress!.ToString();
        
            var session = new Session
            {
                Id = request.SessionId,
                AccessToken = request.AccessToken,
                RefreshToken = request.RefreshToken, 
                DeviceToken = deviceToken,
                DeviceIP = deviceIp,
                UserId = request.UserId
            };

            _context.Add(session);

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
