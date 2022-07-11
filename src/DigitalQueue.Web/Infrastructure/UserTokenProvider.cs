using DigitalQueue.Web.Data.Users;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Infrastructure;

public class UserTokenProvider : TotpSecurityStampBasedTokenProvider<ApplicationUser>
{
    public readonly static string ProviderName = nameof(UserTokenProvider);

    public readonly static string AuthenticationPurposeName = "Authentication";
    public readonly static string UpdateEmailPurposeName = "UpdateEmail";

    public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<ApplicationUser> manager, ApplicationUser applicationUser)
        => Task.FromResult(false);
    
}
