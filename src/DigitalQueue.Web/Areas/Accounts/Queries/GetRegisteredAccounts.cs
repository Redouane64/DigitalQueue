using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Accounts.Services;

using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Queries;

public class GetRegisteredAccounts : IRequest<IEnumerable<UserDto>>
{
    
    public class GetRegisteredAccountsHandler : IRequestHandler<GetRegisteredAccounts, IEnumerable<UserDto>>
    {
        private readonly UsersService _usersService;

        public GetRegisteredAccountsHandler(UsersService usersService)
        {
            _usersService = usersService;
        }
        
        public async Task<IEnumerable<UserDto>> Handle(GetRegisteredAccounts request, CancellationToken cancellationToken)
        {
            return await this._usersService.GetAllUsers();
        }
    }
}
