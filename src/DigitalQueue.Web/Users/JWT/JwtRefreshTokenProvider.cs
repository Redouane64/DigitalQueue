using DigitalQueue.Web.Domain;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DigitalQueue.Web.Users.JWT;

public class JwtRefreshTokenProvider : DataProtectorTokenProvider<User>
{
    public sealed class JwtRefreshTokenProviderOptions : DataProtectionTokenProviderOptions
    {
    }

    public static readonly string ProviderName = "JwtRefreshTokenProvider";
    public static readonly string Purpose = "RefershJwtAccessToken";

    public JwtRefreshTokenProvider(
        IDataProtectionProvider dataProtectionProvider,
        IOptions<JwtRefreshTokenProviderOptions> options,
        ILogger<DataProtectorTokenProvider<User>> logger)
        : base(dataProtectionProvider, options, logger)
    {
    }

    public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user)
        => Task.FromResult(false);
}
