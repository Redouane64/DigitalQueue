using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Accounts.Services;
using DigitalQueue.Web.Data.Entities;

using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Queries;

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

            (User user, IList<string> roles) = await this._usersService.FindUserByEmail(email);

            var claims = await this._usersService.GetUserClaims(user);

            return new UserDto(user, roles, claims);
        }
    }
}
