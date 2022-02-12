using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Services;

public class UsersService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IHttpContextAccessor _httpContext;
    private readonly ILogger<UsersService> _logger;

    public UsersService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContext, ILogger<UsersService> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _httpContext = httpContext;
        _logger = logger;
    }

    public async Task<IList<Claim>?> CreateUser(string email, string password, string role = RoleDefaults.User)
    {
        var defaultUser = new User() { UserName = email.Split("@").First(), Email = email };
        return await CreateUser(defaultUser, password, role);
    }

    public async Task<IList<Claim>?> CreateUser(User user, string password, string role = RoleDefaults.User)
    {
        var createUser = await _userManager.CreateAsync(
            user, password
        );

        if (!createUser.Succeeded)
        {
            _logger.LogWarning("Failed to create user: {0}",
                createUser.Errors.Select(e => e.Description).First());
            return null;
        }

        bool assignUserToRole = await AssignUserToRole(user, role);
        if (!assignUserToRole)
        {
            return null;
        }

        Claim[] claims = {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        bool assignClaimToUser = await AssignClaimToUser(user, claims);
        if (!assignClaimToUser)
        {
            return null;
        }

        return claims;
    }

    public async Task<bool> AssignClaimToUser(User user, Claim[] claims)
    {
        var setClaims = await _userManager.AddClaimsAsync(
            user,
            claims
        );

        if (setClaims.Succeeded)
        {
            return true;
        }

        // at this point we failed
        _logger.LogWarning("Failed to set user claims: {0}",
            setClaims.Errors.Select(e => e.Description).First());

        // clean up invalid user
        await this._userManager.DeleteAsync(user);

        return true;
    }

    public async Task<bool> AssignUserToRole(User user, string role)
    {
        var existingRole = await this._roleManager.FindByNameAsync(role);
        if (existingRole is null)
        {
            return false;
        }
        
        var setRole = await _userManager.AddToRoleAsync(user, role);
        if (setRole.Succeeded)
        {
            return true;
        }

        // at this point we failed.
        _logger.LogWarning("Failed to set user role: {0}",
            setRole.Errors.Select(e => e.Description).First());

        // clean up invalid user
        await this._userManager.DeleteAsync(user);

        return false;
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

    public async Task<(User user, IList<string> role)> FindUserByEmail(string email)
    {
        var user = await this._userManager.FindByEmailAsync(email);
        var role = await this._userManager.GetRolesAsync(user);

        return (user, role);
    }
    
    public async Task<UserDto> FindUserById(string id)
    {
        var user = await this._userManager.FindByIdAsync(id);
        if (user is null)
        {
            return null;
        }
        
        var roles = await this._userManager.GetRolesAsync(user);
        var claims = await this.GetUserClaims(user);
        
        return new UserDto(user, roles, claims);
    }
    
    public async Task<IReadOnlyList<UserDto>> GetAllUsers()
    {
        // TODO: add pagination

        var callerUserId = this._httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var users = this._userManager.Users.Where(user => user.Id != callerUserId);
        var allUsers = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await this._userManager.GetRolesAsync(user);
            IEnumerable<UserClaimDto> userClaims = await GetUserClaims(user);

            allUsers.Add(new UserDto(user, roles, userClaims));
        }

        return allUsers.AsReadOnly();
    }

    public async Task<IEnumerable<UserClaimDto>> GetUserClaims(User user)
    {
        var claims = await this._userManager.GetClaimsAsync(user);

        var userClaims = claims
            .Where(claim => 
                claim.Type is not ClaimTypes.Email && 
                claim.Type is not ClaimTypes.Role &&
                claim.Type is not ClaimTypes.NameIdentifier)
            .GroupBy(claim => claim.Type)
            .Select(entry =>
                new UserClaimDto(entry.Key, entry.Select(claim => claim.Value).ToArray())
            );
        return userClaims;
    }
}
