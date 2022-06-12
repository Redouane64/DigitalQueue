using MediatR;

namespace DigitalQueue.Web.Areas.Sessions.Commands;

public partial class DeleteSessionCommand : IRequest
{
    public string UserId { get; }
    public string SessionSecurityStamp { get; }

    public DeleteSessionCommand(string userId, string sessionSecurityStamp)
    {
        UserId = userId;
        SessionSecurityStamp = sessionSecurityStamp;
    }
}
