using System.Security.Claims;

using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Services.Notifications;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public partial class UpdateUserRolesCommand
{
    public class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly DigitalQueueContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly NotificationService _notificationService;
        private readonly ILogger<UpdateUserRolesCommandHandler> _logger;

        public UpdateUserRolesCommandHandler(
            UserManager<User> userManager,
            DigitalQueueContext context,
            IHttpContextAccessor httpContextAccessor,
            NotificationService notificationService,
            ILogger<UpdateUserRolesCommandHandler> logger)
        {
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == request.User)
            {
                _logger.LogWarning("Current user is not allowed to change their own roles");
                return false;
            }

            using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(request.User);

                    if (user is null)
                    {
                        return false;
                    }

                    foreach (var role in request.Roles)
                    {
                        if (request.Remove)
                        {
                            if (await _userManager.IsInRoleAsync(user, role))
                            {
                                await _userManager.RemoveFromRoleAsync(user, role);
                                await _userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, role));
                            }
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, role);
                            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));
                        }
                    }

                    if (!request.Remove && request.Roles.Contains(RoleDefaults.Administrator))
                    {
                        // account is assigned admin role, create password and send it to account email
                        var password = GeneratePassword();
                        var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var setPasswordResult = await _userManager.ResetPasswordAsync(user, passwordResetToken, password);
                        if (!setPasswordResult.Succeeded)
                        {
                            await transaction.RollbackAsync(cancellationToken);
                            return false;
                        }

                        await _notificationService.Send(
                            new Notification<AdminDashboardPassword>(new AdminDashboardPassword(user.Email, password)));
                    }

                    await transaction.CommitAsync(cancellationToken);

                    return true;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, null);

                    await transaction.RollbackAsync(cancellationToken);

                    return false;
                }

            }
        }

        private string GeneratePassword() => Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("+", String.Empty)
            .Replace("=", String.Empty)
            .Replace("/", String.Empty)[1..8];
    }
}
