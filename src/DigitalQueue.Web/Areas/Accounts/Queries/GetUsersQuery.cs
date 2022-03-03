using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Accounts.Queries;

public class GetUsersQuery : IRequest<IEnumerable<UserDto>>
{
    
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GetUsersQueryHandler> _logger;

        public GetUsersQueryHandler(
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IHttpContextAccessor httpContextAccessor,
            ILogger<GetUsersQueryHandler> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        
        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            // TODO: add pagination

            var currentUser = await this._userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        
            var users = this._userManager.Users
                .Where(user => user.Id != currentUser.Id)
                .OrderByDescending(u => u.CreateAt)
                .ToArray();
            
            var allUsers = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await this._userManager.GetRolesAsync(user);
                var claims = await this._userManager.GetClaimsAsync(user);

                allUsers.Add(new UserDto(user, await ToRolesDtos(roles), ToClaimsDtos(claims))
                {
                    CreatedAt = user.CreateAt
                });
            }

            return allUsers;
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
