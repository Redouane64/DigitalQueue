using DigitalQueue.Web.Areas.Accounts.Dtos;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class SearchRoleCommand : IRequest<SearchResult<RoleDto>>
{
    
    public SearchRoleCommand(string query)
    {
        Query = query;
    }

    public string Query { get; }
    
    public class SearchRoleCommandHandler : IRequestHandler<SearchRoleCommand, SearchResult<RoleDto>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public SearchRoleCommandHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        
        public async Task<SearchResult<RoleDto>> Handle(SearchRoleCommand request, CancellationToken cancellationToken)
        {
            var roles = await _roleManager.Roles
                .Where(role => role.Name.Contains(request.Query))
                .Select(role => new RoleDto(role.Name, role.Id))
                .ToArrayAsync(cancellationToken);
        
            return new SearchResult<RoleDto>(roles);
        }
    }
}
