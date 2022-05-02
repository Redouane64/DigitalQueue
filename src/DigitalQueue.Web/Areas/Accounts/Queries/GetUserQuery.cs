using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Accounts.Queries;

public class GetUserQuery : IRequest<UserDto?>
{
    public string Id { get; }

    public GetUserQuery(string id)
    {
        Id = id;
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto?>
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetUserQueryHandler(IMediator mediator, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _mediator = mediator;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public async Task<UserDto?> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await this._userManager.Users
                .AsNoTracking()
                .Include(e => e.TeacherOf)
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            
            if (user is null)
            {
                return null;
            }
        
            var roles = await this._userManager.GetRolesAsync(user);
            var userCourses = await _mediator.Send(new GetUserPermissionsQuery(user.Id));
            
            return new UserDto(user, await ToAccountRoles(roles), userCourses);
        }
        
        private async Task<AccountRoleDto[]> ToAccountRoles(IEnumerable<string> roles) =>
            await this._roleManager.Roles
                .Where(role => roles.Contains(role.Name))
                .Select(role => new AccountRoleDto(role.Name, role.Id))
                .ToArrayAsync();
        
    }
}
