using System.Security.Claims;

using DigitalQueue.Web.Data.Entities;

namespace DigitalQueue.Web.Areas.Accounts.Services;

public interface IUserService
{
    Task<User?> FindUserByEmail(string email);
    
    Task<User?>  CreateUser(
        string email, 
        string name, 
        IEnumerable<string> roles, 
        Dictionary<string, string> claims, 
        CancellationToken cancellationToken = default);
    
    Task<bool> UpdateUserEmail(ClaimsPrincipal user, string token, string email);

    Task<bool> UpdateUserName(ClaimsPrincipal user, string name);

    Task AddUsersToRoles(string[] users, string[] roles);
    
    Task RemoveUsersFromRoles(string[] users, string[] roles);
}
