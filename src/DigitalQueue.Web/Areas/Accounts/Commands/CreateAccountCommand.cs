using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class CreateAccountCommand : IRequest<AccessTokenDto?>
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [RegularExpression("^[a-zA-Z ]*$")]
    [DataType(DataType.Text)]
    public string FullName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }

    [BindNever]
    public string[]? Roles { get; set; }

    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, AccessTokenDto?>
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtTokenService _tokenService;
        private readonly ILogger<CreateAccountCommandHandler> _logger;

        public CreateAccountCommandHandler(
            UserManager<User> userManager, 
            JwtTokenService tokenService,
            ILogger<CreateAccountCommandHandler> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<AccessTokenDto?> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                Email = request.Email,
                FullName = request.FullName,
                UserName = request.Email.Split("@").First()
            };
            
            var createUser = await _userManager.CreateAsync(
                user, request.Password
            );

            if (!createUser.Succeeded)
            {
                _logger.LogWarning("Unable to create user: {0}",
                    createUser.Errors.Select(e => e.Description).First());
                return null;
            }

            request.Roles = new[] {RoleDefaults.User};
            var assignRole = await _userManager.AddToRolesAsync(user, request.Roles);
            if (!assignRole.Succeeded)
            {
                // at this point we failed.
                _logger.LogWarning("Unable to set user role: {0}",
                    assignRole.Errors.Select(e => e.Description).First());

                // clean up invalid user
                await _userManager.DeleteAsync(user);
                
                return null;
            }

            Claim[] identityClaims = {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var roleClaims = request.Roles.Select(
                role => new Claim(ClaimTypes.Role, role)
            );

            var assignClaims = await _userManager.AddClaimsAsync(user, identityClaims.Concat(roleClaims));
            if (!assignClaims.Succeeded)
            {
                // at this point we failed
                _logger.LogWarning("Unable to set user claims: {0}",
                    assignClaims.Errors.Select(e => e.Description).First());

                // clean up invalid user
                await _userManager.DeleteAsync(user);
                
                return null;
            }

            var (token, refreshToken) = await _tokenService.GenerateToken(
                identityClaims,
                user
            );

            return new AccessTokenDto(token, refreshToken);
        }
    }
}
