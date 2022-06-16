using System.Security.Claims;

using MediatR;

namespace DigitalQueue.Web.Areas.Common;

public class AuthorizedQuery<T> : IRequest<T>
{
    public ClaimsPrincipal User { get; }

    public AuthorizedQuery(ClaimsPrincipal user)
    {
        User = user;
    }
}
