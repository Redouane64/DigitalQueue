using System.Security.Claims;

namespace DigitalQueue.Web.Areas.Accounts.Models;

public enum AuthenticatedUserType
{
    Created,
    Existing
}

public class AuthenticationResult
{
    public ClaimsPrincipal User { get; }
    public AuthenticatedUserType Type { get; set; }

    public AuthenticationResult(ClaimsPrincipal user)
    {
        User = user;
    }
}
