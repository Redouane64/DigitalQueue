using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Models;
using DigitalQueue.Web.Areas.Users.Models;
using DigitalQueue.Web.Data.Users;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Users.Queries;

public class GetAllUsersQuery : IRequest<IEnumerable<UserAccount>>
{
    public ClaimsPrincipal User { get; }

    public GetAllUsersQuery(ClaimsPrincipal user)
    {
        User = user;
    }
}

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserAccount>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetAllUsersQueryHandler(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<UserAccount>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        // TODO: add pagination

        var currentUser = await this._userManager.GetUserAsync(request.User);

        var users = await this._userManager.Users
            .Where(user => user.Id != currentUser.Id)
            .OrderByDescending(u => u.CreateAt)
            .Select(u => new UserAccount { Email = u.Email, Name = u.Name })
            .ToArrayAsync(cancellationToken);
        
        return users;
    }

}

