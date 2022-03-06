using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Common.Dtos;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class SearchRoleCommand : IRequest<SearchResult<AccountRoleDto>>
{
    
    public SearchRoleCommand(string query)
    {
        Query = query;
    }

    public string Query { get; }
    
    public class SearchRoleCommandHandler : IRequestHandler<SearchRoleCommand, SearchResult<AccountRoleDto>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public SearchRoleCommandHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        
        public async Task<SearchResult<AccountRoleDto>> Handle(SearchRoleCommand request, CancellationToken cancellationToken)
        {
            var roles = await _roleManager.Roles
                .Where(role => role.Name.Contains(request.Query))
                .Select(role => new AccountRoleDto(role.Name, role.Name))
                .ToArrayAsync(cancellationToken);
        
            return new SearchResult<AccountRoleDto>(roles);
        }
    }
}
