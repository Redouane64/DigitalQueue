using DigitalQueue.Web.Data.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DigitalQueue.Web.Infrastructure;

public class DigitalQueueUserManager : UserManager<User>
{
    public override bool SupportsUserLockout => false;
    public override bool SupportsUserPhoneNumber => false;
    public override bool SupportsUserTwoFactor => false;
    public override bool SupportsUserLogin => false;
    public override bool SupportsUserAuthenticatorKey => false;
    public override bool SupportsUserTwoFactorRecoveryCodes => false;

    public DigitalQueueUserManager(
        IUserStore<User> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<User> passwordHasher,
        IEnumerable<IUserValidator<User>> userValidators,
        IEnumerable<IPasswordValidator<User>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<DigitalQueueUserManager> logger)
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
    }
}
