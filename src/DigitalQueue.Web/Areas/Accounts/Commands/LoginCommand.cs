using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class LoginCommand : IRequest<AccessTokenDto?>
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    public class LogInCommandHandler : IRequestHandler<LoginCommand, AccessTokenDto?>
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtTokenService _tokenService;
        private readonly ILogger<LogInCommandHandler> _logger;

        public LogInCommandHandler(UserManager<User> userManager, JwtTokenService tokenService, ILogger<LogInCommandHandler> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _logger = logger;
        }
        
        public async Task<AccessTokenDto?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
        
            if (user is null)
            {
                _logger.LogWarning("Unable to find user '{0}'", request.Email);
                return null;
            }

            var correct = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!correct)
            {
                _logger.LogWarning("Unable to authenticate user '{0}' reason: incorrect password", request.Email);
                return null;
            }

            var claims = new[] {new Claim(ClaimTypes.NameIdentifier, user.Id)};
            
            var (token, refreshToken) = await _tokenService.GenerateToken(claims,
                new User {Email = request.Email});

            return new AccessTokenDto(token, refreshToken);
        }
    }
}
