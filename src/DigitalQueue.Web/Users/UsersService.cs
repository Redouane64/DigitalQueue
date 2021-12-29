using DigitalQueue.Web.Domain;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Users;

public class UsersService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsersService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task CreateDefaultAccountsAndRoles()
    {
        await _roleManager.CreateAsync(RoleDefaults.Administrator);
        await _roleManager.CreateAsync(RoleDefaults.Teacher);
        await _roleManager.CreateAsync(RoleDefaults.Student);
        
        await _userManager.CreateAsync(
            new User() { UserName = "redouane", Email = "redouane.sobaihi@live.com" }, 
            "password"
        );
        
    }
}
