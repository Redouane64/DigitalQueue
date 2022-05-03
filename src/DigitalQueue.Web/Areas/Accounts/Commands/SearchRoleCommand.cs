using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Common.Dtos;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class SearchRoleCommand : IRequest<SearchResult<AccountRoleDto>>
{
    public SearchRoleCommand(string? query = null, IEnumerable<string>? filter = null, bool returnIds = false)
    {
        ReturnIds = returnIds;
        Query = query;
        Filter = filter;
    }

    public bool ReturnIds { get; }
    public string? Query { get; }
    public IEnumerable<string>? Filter { get; }

    public class SearchRoleCommandHandler : IRequestHandler<SearchRoleCommand, SearchResult<AccountRoleDto>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public SearchRoleCommandHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<SearchResult<AccountRoleDto>> Handle(SearchRoleCommand request,
            CancellationToken cancellationToken)
        {
            var query = _roleManager.Roles.AsNoTracking();

            if (request.Filter is not null)
            {
                query = query.Where(role => request.Filter.Contains(role.Name));
            }
            
            if (!String.IsNullOrEmpty(request.Query))
            {
                query = query.Where(role => role.Name.Contains(request.Query));
            }

            AccountRoleDto[] roles;
            if (request.ReturnIds)
            {
                roles = await query
                    .Select(role => new AccountRoleDto(role.Name, role.Id))
                    .ToArrayAsync(cancellationToken);

                return new SearchResult<AccountRoleDto>(roles);
            }

            roles = await query
                .Select(role => new AccountRoleDto(role.Name, role.Name))
                .ToArrayAsync(cancellationToken);

            return new SearchResult<AccountRoleDto>(roles);
        }
    }
}
