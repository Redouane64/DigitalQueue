using System.Security.Claims;

using DigitalQueue.Web.Users.Dtos;

using MediatR;

namespace DigitalQueue.Web.Users.Queries;

public class GetProfileRequest : IRequest<UserDto?>
{
    public ClaimsPrincipal User { get; }

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
            var email = request.User.FindFirstValue(ClaimTypes.Email);

            if (email is null)
            {
                return null;
            }

            var (user, roles) = await this._usersService.FindUserByEmail(email);
            
            if (user is null)
            {
                return null;
            }
            
            return new UserDto(user.Email, user.UserName, roles);
        }
    }
}
