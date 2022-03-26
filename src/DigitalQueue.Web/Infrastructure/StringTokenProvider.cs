using DigitalQueue.Web.Data.Entities;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DigitalQueue.Web.Infrastructure;

public class StringTokenProvider : DataProtectorTokenProvider<User>
{
    public readonly static string ProviderName = nameof(StringTokenProvider);

    public StringTokenProvider(
        IDataProtectionProvider dataProtectionProvider, 
        IOptions<DataProtectionTokenProviderOptions> options, 
        ILogger<StringTokenProvider> logger) 
        : base(dataProtectionProvider, options, logger)
    {
    }
    
    public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user) 
        => Task.FromResult(false);

}
