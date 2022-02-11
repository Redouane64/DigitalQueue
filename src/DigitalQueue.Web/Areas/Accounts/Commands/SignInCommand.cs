using System.ComponentModel.DataAnnotations;

using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Accounts.Services;
using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Infrastructure;

using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class SignInCommand : IRequest<AccessTokenDto?>
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public class SignInCommandHandler : IRequestHandler<SignInCommand, AccessTokenDto?>
    {
        private readonly UsersService _usersService;
        private readonly JwtTokenService _tokenService;

        public SignInCommandHandler(UsersService usersService, JwtTokenService tokenService)
        {
            _usersService = usersService;
            _tokenService = tokenService;
        }
        
        public async Task<AccessTokenDto?> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var claims = await _usersService.GetUserClaims(request.Email, request.Password);

            if (claims is not null)
            {
                var (token, refreshToken) = await _tokenService.GenerateToken(claims,
                    new User {Email = request.Email});

                return new AccessTokenDto(token, refreshToken);
            }

            return null;
        }
    }
}
