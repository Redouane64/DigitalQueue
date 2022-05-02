using System.Security.Claims;

using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Infrastructure;
using DigitalQueue.Web.Services.Notifications;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class CreateUserAuthenticationTokenCommand : IRequest
{
    public string Email { get; }

    public CreateUserAuthenticationTokenCommand(string email)
    {
        Email = email;
    }
    
    public class CreateUserAuthenticationTokenCommandHandler : IRequestHandler<CreateUserAuthenticationTokenCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly DigitalQueueContext _context;
        private readonly NotificationService _notificationService;
        private readonly ILogger<CreateUserAuthenticationTokenCommandHandler> _logger;

        public CreateUserAuthenticationTokenCommandHandler(UserManager<User> userManager, DigitalQueueContext context, NotificationService notificationService, ILogger<CreateUserAuthenticationTokenCommandHandler> logger)
        {
            _userManager = userManager;
            _context = context;
            _notificationService = notificationService;
            _logger = logger;
        }
        
        public async Task<Unit> Handle(CreateUserAuthenticationTokenCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await this._context.Database.BeginTransactionAsync(cancellationToken);
            
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                // create user.
                user = new User { Email = request.Email };
                var createUserResult = await _userManager.CreateAsync(user);

                if (!createUserResult.Succeeded)
                {
                    var error = createUserResult.Errors.Select(e => e.Description).FirstOrDefault() ?? "(null)";
                    _logger.LogWarning("Unable to create user with email '{Email}': {error}", request.Email, error);
                    
                    // TODO: role back transaction
                    await transaction.RollbackAsync(cancellationToken);
                    
                    return Unit.Value;
                }

                // assign user to default role
                var assignRoleResult = await _userManager.AddToRoleAsync(user, RoleDefaults.User);
                if (!assignRoleResult.Succeeded)
                {
                    var error = assignRoleResult.Errors.Select(e => e.Description).FirstOrDefault() ?? "(null)";
                    _logger.LogWarning("Unable to assign default role to user '{Email}': {error}", request.Email, error);
                    
                    // TODO: role back transaction
                    await transaction.RollbackAsync(cancellationToken);
                    
                    return Unit.Value;
                }
                
                // create default claims for user
                var setClaimsResult = await _userManager.AddClaimsAsync(user,
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Role, RoleDefaults.User),
                    });
                if (!setClaimsResult.Succeeded)
                {
                    var error = setClaimsResult.Errors.Select(e => e.Description).FirstOrDefault() ?? "(null)";
                    _logger.LogWarning("Unable to assign default claims to user '{Email}': {error}", request.Email, error);
                    
                    // TODO: role back transaction
                    await transaction.RollbackAsync(cancellationToken);
                    
                    return Unit.Value;
                }
            }
            
            var token = await _userManager.GenerateUserTokenAsync(user, AuthenticationTokenProvider.ProviderName, "authentication");

            try
            {
                await _notificationService.Send(new Notification<VerificationToken>(new VerificationToken(request.Email, token)));
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Unable to send authentication code");
                
                await transaction.RollbackAsync(cancellationToken);
                return Unit.Value;
            }
            
            await transaction.CommitAsync(cancellationToken);
            
            _logger.LogInformation("User '{Email}' created successfully", user.Email);
            return Unit.Value;
        }
    }
}
