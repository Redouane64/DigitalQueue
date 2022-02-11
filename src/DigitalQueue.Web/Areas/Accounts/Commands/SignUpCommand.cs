using System.ComponentModel.DataAnnotations;

using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Infrastructure;
using DigitalQueue.Web.Users;

using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class SignUpCommand : IRequest<AccessTokenDto?>
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [RegularExpression("^[a-zA-Z ]*$")]
    [DataType(DataType.Text)]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }
    
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, AccessTokenDto?>
    {
        private readonly UsersService _usersService;
        private readonly JwtTokenService _tokenService;

        public SignUpCommandHandler(UsersService usersService, JwtTokenService tokenService)
        {
            _usersService = usersService;
            _tokenService = tokenService;
        }

        public async Task<AccessTokenDto?> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var claims = await _usersService.CreateUser(
                new User()
                {
                    Email = request.Email,
                    UserName = request.UserName
                }, 
                request.Password, 
                RoleDefaults.Student);

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
