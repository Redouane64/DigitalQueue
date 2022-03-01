using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Dtos;
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

        public GetUserQueryHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
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
            var claims = await this._userManager.GetClaimsAsync(user);

            return new UserDto(user, await ToRolesDtos(roles), ToClaimsDtos(claims));
        }
        
        private async Task<RoleDto[]> ToRolesDtos(IEnumerable<string> roles) =>
            await this._roleManager.Roles
                .Where(role => roles.Contains(role.Name))
                .Select(role => new RoleDto(role.Name, role.Id))
                .ToArrayAsync();

        private IEnumerable<ClaimDto> ToClaimsDtos(IEnumerable<Claim> claims) => claims
            .Where(claim => 
                claim.Type is not ClaimTypes.Email && 
                claim.Type is not ClaimTypes.Role &&
                claim.Type is not ClaimTypes.NameIdentifier)
            .GroupBy(claim => claim.Type)
            .Select(entry =>
                new ClaimDto(entry.Key, entry.Select(claim => claim.Value).ToArray())
            );
    }
}
