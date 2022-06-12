using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using MediatR;

namespace DigitalQueue.Web.Areas.Sessions.Commands;

public partial class CreateSessionCommand
{
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

            var session = new Session
            {
                AccessToken = request.AccessToken,
                RefreshToken = request.RefreshToken,
                SecurityStamp = request.SecurityStamp,
                UserId = request.UserId,
                DeviceToken = deviceToken,
            };

            _context.Add(session);

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
