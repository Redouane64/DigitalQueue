using System.Security.Claims;

using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Users;
using DigitalQueue.Web.Infrastructure;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Accounts.Services;

public class UsersService : IUserService
{
    
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly DigitalQueueContext _context;

    public UsersService(UserManager<ApplicationUser> userManager, DigitalQueueContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<ApplicationUser?> FindUserByEmail(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<ApplicationUser?> CreateUser(
        string email, 
        string name, 
        IEnumerable<string> roles, 
        Dictionary<string, string> claims, 
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await this._context.Database.BeginTransactionAsync(cancellationToken);

        var user = new ApplicationUser { Email = email, UserName = name };
        var createUserResult = await _userManager.CreateAsync(user);

        if (!createUserResult.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);
            return null;
        }

        // assign user to default role
        var assignRoleResult = await _userManager.AddToRolesAsync(user, roles);
        if (!assignRoleResult.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);
            return null;
        }

        // create default claims for user
        var setClaimsResult = await _userManager.AddClaimsAsync(user,
            claims.Select(entry => new Claim(entry.Key, entry.Value)));
        
        if (!setClaimsResult.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);
            return null;
        }

        await transaction.CommitAsync(cancellationToken);
        
        return user;
    }

    public async Task<bool> UpdateUserEmail(ClaimsPrincipal user, string token, string email)
    {
        var _user = await _userManager.GetUserAsync(user);

        if (_user is null)
        {
            return false;
        }

        var isValidToken = await _userManager.VerifyUserTokenAsync(
            _user,
            UserTokenProvider.ProviderName,
            UserTokenProvider.AuthenticationPurposeName, token);

        if (!isValidToken)
        {
            return false;
        }

        if (_user.Email.Equals(email))
        {
            return false;
        }

        var changeEmailToken = await _userManager.GenerateChangeEmailTokenAsync(_user, email);
        var updateEmailResult = await _userManager.ChangeEmailAsync(_user, email, changeEmailToken);

        return updateEmailResult.Succeeded;
    }

    public async Task<bool> UpdateUserName(ClaimsPrincipal user, string name)
    {
        var _user = await _userManager.GetUserAsync(user);
        if (_user is null)
        {
            return false;
        }
        
        var result = await _userManager.SetUserNameAsync(_user, name);

        return result.Succeeded;
    }

    public async Task AddUsersToRoles(string[] users, string[] roles)
    {
        var _users = await _userManager.Users.Where(u =>
            users.Contains(u.Id) || users.Select(u => u.ToUpperInvariant()).Contains(u.NormalizedEmail)
        ).ToArrayAsync();
        
        foreach (var user in _users)
        {
            await _userManager.AddToRolesAsync(user, roles);
        }
    }

    public async Task RemoveUsersFromRoles(string[] users, string[] roles)
    {
        var _users = await _userManager.Users.Where(u =>
            users.Contains(u.Id) || users.Select(u => u.ToUpperInvariant()).Contains(u.NormalizedEmail)
        ).ToArrayAsync();
        
        foreach (var user in _users)
        {
            await _userManager.RemoveFromRolesAsync(user, roles);
        }
    }
    
}
