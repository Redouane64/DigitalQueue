using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Accounts.Services;

using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Queries;

public class GetProfileByIdRequest : IRequest<UserDto?>
{
    public GetProfileByIdRequest(string id)
    {
        Id = id;
    }
    
    public string Id { get; }
    
    public class GetProfileByIdRequestHandler : IRequestHandler<GetProfileByIdRequest, UserDto?>
    {
        private readonly UsersService _usersService;

        public GetProfileByIdRequestHandler(UsersService usersService)
        {
            _usersService = usersService;
        }
        
        public async Task<UserDto?> Handle(GetProfileByIdRequest request, CancellationToken cancellationToken)
        {
            return await this._usersService.FindUserById(request.Id);
        }
    }
}
