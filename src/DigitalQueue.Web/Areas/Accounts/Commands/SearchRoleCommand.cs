using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Accounts.Services;

using MediatR;

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
        private readonly UsersService _usersService;

        public SearchRoleCommandHandler(UsersService usersService)
        {
            _usersService = usersService;
        }
        
        public async Task<SearchResult<RoleDto>> Handle(SearchRoleCommand request, CancellationToken cancellationToken)
        {
            return await this._usersService.FindRoles(request.Query);
        }
    }
}
