using DigitalQueue.Web.Data.Entities;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Infrastructure;

public class UserTokenProvider : TotpSecurityStampBasedTokenProvider<User>
{
    public readonly static string ProviderName = nameof(UserTokenProvider);

    public readonly static string AuthenticationPurposeName = "Authentication";
    public readonly static string UpdateEmailPurposeName = "UpdateEmail";

    public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user)
        => Task.FromResult(false);
    
}
