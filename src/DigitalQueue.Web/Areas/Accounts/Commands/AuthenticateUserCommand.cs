using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Common.Interfaces;
using DigitalQueue.Web.Areas.Sessions.Commands;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class AuthenticateUserCommand : IAuthenticationCommand<AccessTokenDto?>
{
    public AuthenticateUserCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; }
    
    public string Password { get; }

    public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, AccessTokenDto?>
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtTokenService _tokenService;
        private readonly IMediator _mediator;
        private readonly ILogger<AuthenticateUserCommandHandler> _logger;

        public AuthenticateUserCommandHandler(
            UserManager<User> userManager, 
            JwtTokenService tokenService,
            IMediator mediator,
            ILogger<AuthenticateUserCommandHandler> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mediator = mediator;
            _logger = logger;
        }
        
        public async Task<AccessTokenDto?> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
        
            if (user is null)
            {
                _logger.LogWarning("Unable to find user '{Email}'", request.Email);
                return null;
            }

            var correct = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!correct)
            {
                _logger.LogWarning("Unable to authenticate user '{Email}' reason: incorrect password", request.Email);
                return null;
            }

            var sessionId = Guid.NewGuid().ToString();
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypesDefaults.Session, sessionId)
            };
            
            var (accessToken, refreshToken) = await _tokenService.GenerateToken(claims, user);

            await _mediator.Send(new CreateSessionCommand(user.Id, sessionId, accessToken, refreshToken), cancellationToken);
            
            return new AccessTokenDto(accessToken, refreshToken);
        }
    }
}
