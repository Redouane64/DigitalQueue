using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Accounts.Services;

using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Queries;

public class GetProfileRequest : IRequest<UserDto?>
{
    public ClaimsPrincipal? User { get; }

    public string? Id { get; }

    public GetProfileRequest(string? id)
    {
        Id = id;
    }

    public GetProfileRequest(ClaimsPrincipal user)
    {
        User = user;
    }
    
    public class GetProfileRequestHandler : IRequestHandler<GetProfileRequest, UserDto?>
    {
        private readonly UsersService _usersService;

        public GetProfileRequestHandler(UsersService usersService)
        {
            _usersService = usersService;
        }
        
        public async Task<UserDto?> Handle(GetProfileRequest request, CancellationToken cancellationToken)
        {
            if (request.Id is not null)
            {
                return await this._usersService.FindUserById(request.Id);
            }
            
            var userId = request.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return null;
            }

            return await this._usersService.FindUserById(userId);
        }
    }
}
