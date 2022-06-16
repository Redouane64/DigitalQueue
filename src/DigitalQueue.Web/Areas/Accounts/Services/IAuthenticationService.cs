using System.Security.Claims;

namespace DigitalQueue.Web.Areas.Accounts.Services;

public interface IAuthenticationService
{
    Task<string?> CreateUserToken(ClaimsPrincipal user, string tokenPurpose);

    string? CreateAccessToken(IEnumerable<Claim> claims);

    Task<string?> CreateRefreshToken(ClaimsPrincipal user);

    Task<bool> ValidateRefreshToken(string token, ClaimsPrincipal user);
}
