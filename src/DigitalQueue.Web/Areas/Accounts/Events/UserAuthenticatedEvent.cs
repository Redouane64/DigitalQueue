using System.Security.Claims;

using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Events;

public class UserAuthenticatedEvent : INotification
{
    public ClaimsPrincipal User { get; }
    public Dictionary<string, string>? Data { get; }

    public UserAuthenticatedEvent(ClaimsPrincipal user)
    {
        User = user;
    }

    public UserAuthenticatedEvent(ClaimsPrincipal user, Dictionary<string, string> data)
    {
        User = user;
        Data = data;
    }
}
