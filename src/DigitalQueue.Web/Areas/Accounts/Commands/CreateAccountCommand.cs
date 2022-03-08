using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class CreateAccountCommand : IRequest<AccessTokenDto?>
{
    protected bool _isActive = false;
    protected string[] _roles = null;

    public CreateAccountCommand()
    { }

    public CreateAccountCommand(string[] roles, bool isActive = false)
    {
        _isActive = isActive;
        _roles = roles;
    }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [RegularExpression("^[a-zA-Z ]*$")]
    [DataType(DataType.Text)]
    public string FullName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }
    
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, AccessTokenDto?>
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtTokenService _tokenService;
        private readonly DigitalQueueContext _context;
        private readonly ILogger<CreateAccountCommandHandler> _logger;

        public CreateAccountCommandHandler(
            UserManager<User> userManager, 
            JwtTokenService tokenService,
            DigitalQueueContext context,
            ILogger<CreateAccountCommandHandler> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _context = context;
            _logger = logger;
        }

        public async Task<AccessTokenDto?> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var user = new User()
                {
                    Email = request.Email,
                    FullName = request.FullName,
                    UserName = request.Email,
                    IsActive = request._isActive,
                };
            
                var createUser = await _userManager.CreateAsync(
                    user, request.Password
                );

                if (!createUser.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    
                    _logger.LogWarning("Unable to create user: {0}",
                        createUser.Errors.Select(e => e.Description).First());
                    return null;
                }

                request._roles ??= new[] { RoleDefaults.User };
                var assignRole = await _userManager.AddToRolesAsync(user, request._roles);
                if (!assignRole.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    
                    _logger.LogWarning("Unable to set user role: {0}",
                        assignRole.Errors.Select(e => e.Description).First());
                    return null;
                }

                Claim[] identityClaims = {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };
                var roleClaims = request._roles.Select(
                    role => new Claim(ClaimTypes.Role, role)
                );

                var assignClaims = await _userManager.AddClaimsAsync(user, identityClaims.Concat(roleClaims));
                if (!assignClaims.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    
                    _logger.LogWarning("Unable to set user claims: {0}",
                        assignClaims.Errors.Select(e => e.Description).First());
                    return null;
                }

                await transaction.CommitAsync(cancellationToken);

                var (token, refreshToken) = await _tokenService.GenerateToken(
                    identityClaims,
                    user
                );

                return new AccessTokenDto(token, refreshToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unable to create user");
            }

            return null;
        }
    }
}
