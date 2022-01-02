using System.Security.Claims;

using DigitalQueue.Web.Data;
using DigitalQueue.Web.Domain;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Users;

public class UsersService
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<UsersService> _logger;

    public UsersService(UserManager<User> userManager, ILogger<UsersService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<IList<Claim>?> CreateUser(string email, string password, string role = RoleDefaults.Student)
    {
        var defaultUser = new User() { UserName = email.Split("@").First(), Email = email };

        var createUser = await _userManager.CreateAsync(
            defaultUser, password
        );

        if (!createUser.Succeeded)
        {
            _logger.LogWarning("Failed to create user: {0}", 
                createUser.Errors.Select(e => e.Description).First());
            return null;
        }

        var setRole = await _userManager.AddToRoleAsync(defaultUser, role);
        if (!setRole.Succeeded)
        {
            _logger.LogWarning("Failed to set user role: {0}", 
                setRole.Errors.Select(e => e.Description).First());
            return null;
        }

        Claim[] claims = new[]
        {
            new Claim(ClaimTypes.Email, defaultUser.Email),
            new Claim(ClaimTypes.Role, role)
        };
        
        var setClaims = await _userManager.AddClaimsAsync(
            defaultUser,
            claims
        );

        if (!setClaims.Succeeded)
        {
            _logger.LogWarning("Failed to user claims: {0}", 
                setClaims.Errors.Select(e => e.Description).First());
            return null;
        }
        
        return claims;
    }

    public async Task<IList<Claim>?> GetUserClaims(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            _logger.LogWarning("Failed to find user '{0}'", email);
            return null;
        }

        var correct = await _userManager.CheckPasswordAsync(user, password);
        if (!correct)
        {
            _logger.LogWarning("Failed to authenticate user '{0}' reason: bad password", email);
            return null;
        }

        return await _userManager.GetClaimsAsync(user);
    }

}
