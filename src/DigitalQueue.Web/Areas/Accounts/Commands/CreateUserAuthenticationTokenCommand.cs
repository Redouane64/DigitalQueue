using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Infrastructure;
using DigitalQueue.Web.Services.Notifications;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class CreateUserAuthenticationTokenCommand : IRequest<AuthenticationStatusDto?>
{
    public string Email { get; }
    public string? DeviceToken { get; }

    public CreateUserAuthenticationTokenCommand(string email, string? deviceToken = null)
    {
        Email = email;
        DeviceToken = deviceToken;
    }
    
    public class CreateUserAuthenticationTokenCommandHandler : IRequestHandler<CreateUserAuthenticationTokenCommand, AuthenticationStatusDto?>
    {
        private readonly DigitalQueueUserManager _userManager;
        private readonly DigitalQueueContext _context;
        private readonly NotificationService _notificationService;
        private readonly FirebaseNotificationService _firebaseNotificationService;
        private readonly ILogger<CreateUserAuthenticationTokenCommandHandler> _logger;

        public CreateUserAuthenticationTokenCommandHandler(
            DigitalQueueUserManager userManager, 
            DigitalQueueContext context, 
            NotificationService notificationService, 
            FirebaseNotificationService firebaseNotificationService,
            ILogger<CreateUserAuthenticationTokenCommandHandler> logger)
        {
            _userManager = userManager;
            _context = context;
            _notificationService = notificationService;
            _firebaseNotificationService = firebaseNotificationService;
            _logger = logger;
        }
        
        public async Task<AuthenticationStatusDto?> Handle(CreateUserAuthenticationTokenCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await this._context.Database.BeginTransactionAsync(cancellationToken);
            
            var user = await _userManager.FindByEmailAsync(request.Email);
            var result = new AuthenticationStatusDto();
            if (user is null)
            {
                // create user.
                user = new User { Email = request.Email, UserName = request.Email, Name = request.Email };
                var createUserResult = await _userManager.CreateAsync(user);

                if (!createUserResult.Succeeded)
                {
                    var error = createUserResult.Errors.Select(e => e.Description).FirstOrDefault() ?? "(null)";
                    _logger.LogWarning("Unable to create user with email '{Email}': {error}", request.Email, error);
                    
                    // TODO: role back transaction
                    await transaction.RollbackAsync(cancellationToken);

                    return null;
                }

                // assign user to default role
                var assignRoleResult = await _userManager.AddToRoleAsync(user, RoleDefaults.User);
                if (!assignRoleResult.Succeeded)
                {
                    var error = assignRoleResult.Errors.Select(e => e.Description).FirstOrDefault() ?? "(null)";
                    _logger.LogWarning("Unable to assign default role to user '{Email}': {error}", request.Email, error);
                    
                    // TODO: role back transaction
                    await transaction.RollbackAsync(cancellationToken);
                    
                    return null;
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
                    await transaction.RollbackAsync(cancellationToken);
                    return null;
                }

                result.Created = true;
            }
            
            var token = await _userManager.GenerateUserTokenAsync(user, AuthenticationTokenProvider.ProviderName
                , AuthenticationTokenProvider.AuthenticationPurposeName);

            bool authCodeSent = true;
            if (request.DeviceToken is not null && !result.Created)
            {
                try
                {
                    await _firebaseNotificationService.Send(new FirebaseNotification(
                        new[] { request.DeviceToken },
                        "Authentication code",
                        $"Your authentication code: {token}"
                    ));
                }
                catch(Exception e)
                {
                    _logger.LogError(e, "Unable to send firebase notification");
                    authCodeSent = false;
                }
            }

            if (!authCodeSent)
            {
                try
                {
                    await _notificationService.Send(new Notification<VerificationToken>(new VerificationToken(request.Email, token)));
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, "Unable to send authentication code");
                
                    await transaction.RollbackAsync(cancellationToken);
                    return null;
                }
            }
            
            await transaction.CommitAsync(cancellationToken);
            
            _logger.LogInformation("User '{Email}' authenticated successfully", user.Email);
            return result;
        }
    }
}
