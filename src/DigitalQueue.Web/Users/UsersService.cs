using System.Security.Claims;

using DigitalQueue.Web.Data;
using DigitalQueue.Web.Domain;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Users;

public class UsersService
{
    private readonly UserManager<User> _userManager;

    public UsersService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task CreateDefaultUser(string email, string password)
    {
        var defaultUser = new User() { UserName = email.Split("@").First(), Email = email };

        await _userManager.CreateAsync(
            defaultUser, password
        );

        await _userManager.AddToRoleAsync(defaultUser, RoleDefaults.Administrator);
        await _userManager.AddClaimsAsync(
            defaultUser,
            new[]
            {
                new Claim(ClaimTypes.Email, defaultUser.Email),
                new Claim(ClaimTypes.Role, RoleDefaults.Administrator)
            }
        );
    }
}
