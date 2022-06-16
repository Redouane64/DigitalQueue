using System.Security.Claims;

using MediatR;

namespace DigitalQueue.Web.Areas.Common;

public class AuthorizedCommand<T> : IRequest<T>
{
    public ClaimsPrincipal User { get; }

    public AuthorizedCommand(ClaimsPrincipal user)
    {
        User = user;
    }
}
