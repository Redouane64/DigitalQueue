using DigitalQueue.Web.Data.Entities;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DigitalQueue.Web.Infrastructure;

public class JwtRefreshTokenProvider : DataProtectorTokenProvider<User>
{
    public sealed class JwtRefreshTokenProviderOptions : DataProtectionTokenProviderOptions
    {
    }

    public static readonly string ProviderName = "JwtRefreshTokenProvider";
    public static readonly string Purpose = "RefreshJwtAccessToken";

    public JwtRefreshTokenProvider(
        IDataProtectionProvider dataProtectionProvider,
        IOptions<JwtRefreshTokenProviderOptions> options,
        ILogger<JwtRefreshTokenProvider> logger)
        : base(dataProtectionProvider, options, logger)
    {
    }

    public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user)
        => Task.FromResult(false);
}
