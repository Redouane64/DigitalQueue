using System.ComponentModel.DataAnnotations;

using DigitalQueue.Web.Domain;
using DigitalQueue.Web.Users.Dtos;
using DigitalQueue.Web.Users.JWT;

using MediatR;

namespace DigitalQueue.Web.Users.Commands;

public class SignInCommand : IRequest<AccessTokenResultDto?>
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public class SignInCommandHandler : IRequestHandler<SignInCommand, AccessTokenResultDto?>
    {
        private readonly UsersService _usersService;
        private readonly JwtTokenService _tokenService;

        public SignInCommandHandler(UsersService usersService, JwtTokenService tokenService)
        {
            _usersService = usersService;
            _tokenService = tokenService;
        }
        
        public async Task<AccessTokenResultDto?> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var claims = await _usersService.GetUserClaims(request.Email, request.Password);

            if (claims is not null)
            {
                var (token, refreshToken) = await _tokenService.GenerateToken(claims,
                    new User {Email = request.Email});

                return new AccessTokenResultDto(token, refreshToken);
            }

            return null;
        }
    }
}
