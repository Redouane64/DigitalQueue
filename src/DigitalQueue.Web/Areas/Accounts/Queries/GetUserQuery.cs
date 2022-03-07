using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Courses.Dtos;
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
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DigitalQueueContext _context;

        public GetUserQueryHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, DigitalQueueContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        
        public async Task<UserDto?> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await this._userManager.Users
                .AsNoTracking()
                .Include(e => e.TeacherOf)
                .Where(u => !u.Archived)
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            
            if (user is null)
            {
                return null;
            }
        
            var roles = await this._userManager.GetRolesAsync(user);
            var claims = await this._userManager.GetClaimsAsync(user);

            var userCourses = ToCourseRoles(claims);
            return new UserDto(user, await ToAccountRoles(roles), userCourses);
        }
        
        private async Task<AccountRoleDto[]> ToAccountRoles(IEnumerable<string> roles) =>
            await this._roleManager.Roles
                .Where(role => roles.Contains(role.Name))
                .Select(role => new AccountRoleDto(role.Name, role.Id))
                .ToArrayAsync();

        private IEnumerable<UserCourseRolesDto> ToCourseRoles(IEnumerable<Claim> claims) => claims
            .Where(claim => 
                claim.Type.Equals(ClaimTypesDefaults.Student) || 
                claim.Type.Equals(ClaimTypesDefaults.Teacher)
            )
            .GroupBy(claim => claim.Value)
            .Select(entry =>
                new UserCourseRolesDto(entry.Key, entry.Select(claim => claim.Type).ToArray())
            );
    }
}
