using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Commands.Sessions;

public class CreateSessionCommand : IRequest
{
    public string UserId { get; set; }
    public string SecurityStamp { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string DeviceToken { get; set; }
    
}

public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand>
{
    private readonly DigitalQueueContext _context;

    public CreateSessionCommandHandler(DigitalQueueContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {

        var session = new Session
        {
            AccessToken = request.AccessToken,
            RefreshToken = request.RefreshToken,
            SecurityStamp = request.SecurityStamp,
            UserId = request.UserId,
            DeviceToken = request.DeviceToken,
        };

        _context.Add(session);

        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}

