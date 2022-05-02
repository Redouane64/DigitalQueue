using DigitalQueue.Web.Data.Entities;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Infrastructure;

public class AuthenticationTokenProvider : TotpSecurityStampBasedTokenProvider<User>
{
    public readonly static string ProviderName = nameof(AuthenticationTokenProvider);
    
    public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user) 
        => Task.FromResult(false);

    // HACK: this is dummy class wrapper around TotpSecurityStampBasedTokenProvider
    //       this token provider is used to generate 6-digits tokens
    
}
