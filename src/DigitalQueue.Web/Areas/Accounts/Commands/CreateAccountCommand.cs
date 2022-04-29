using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Accounts.Events;
using DigitalQueue.Web.Areas.Common.Interfaces;
using DigitalQueue.Web.Areas.Sessions.Commands;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class CreateAccountCommand : IAuthenticationCommand<AccessTokenDto?>
{
    private readonly string[] _roles;
    private readonly bool _isActive;

    public CreateUserDto User { get; }
    
    public CreateAccountCommand(CreateUserDto user, string[]? roles = null, bool isActive = false)
    {
        User = user;
        _roles = (roles ?? Array.Empty<string>()).Union(new[] {RoleDefaults.User}).ToArray();
        _isActive = isActive;
    }

    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, AccessTokenDto?>
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtTokenService _tokenService;
        private readonly DigitalQueueContext _context;
        private readonly IMediator _mediator;
        private readonly ILogger<CreateAccountCommandHandler> _logger;

        public CreateAccountCommandHandler(
            UserManager<User> userManager, 
            JwtTokenService tokenService,
            DigitalQueueContext context,
            IMediator mediator,
            ILogger<CreateAccountCommandHandler> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _context = context;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<AccessTokenDto?> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var user = new User()
                {
                    Email = request.User.Email,
                    Name = request.User.FullName,
                    UserName = request.User.Email,
                    EmailConfirmed = request._isActive
                };
            
                var createUser = await _userManager.CreateAsync(
                    user, request.User.Password
                );

                if (!createUser.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    
                    _logger.LogWarning("Unable to create user: {0}",
                        createUser.Errors.Select(e => e.Description).First());
                    return null;
                }
                
                var assignRole = await _userManager.AddToRolesAsync(user, request._roles);
                if (!assignRole.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    
                    _logger.LogWarning("Unable to set user role: {0}",
                        assignRole.Errors.Select(e => e.Description).First());
                    return null;
                }

                var sessionId = Guid.NewGuid().ToString();
                var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypesDefaults.Session, sessionId)
                };
                var roleClaims = request._roles.Select(
                    role => new Claim(ClaimTypes.Role, role)
                );

                var assignClaims = await _userManager.AddClaimsAsync(user, claims.Concat(roleClaims));
                if (!assignClaims.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    var error = assignClaims.Errors.Select(e => e.Description).First();
                    _logger.LogWarning("Unable to set user claims: {error}", error);
                    
                    return null;
                }

                await transaction.CommitAsync(cancellationToken);

                var (accessToken, refreshToken) = await _tokenService.GenerateToken(
                    claims,
                    user
                );

                if (!user.EmailConfirmed)
                {
                    await this._mediator.Publish(new AccountCreatedEvent(user.Id, user.Email), cancellationToken);
                }

                await _mediator.Send(new CreateSessionCommand(user.Id, sessionId, accessToken, refreshToken), cancellationToken);

                return new AccessTokenDto(accessToken, refreshToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unable to create user");
            }

            return null;
        }
    }
}
