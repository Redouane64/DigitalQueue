using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Services.Notifications;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public partial class CreatePasswordResetTokenCommand
{
    public class CreatePasswordResetTokenCommandHandler : IRequestHandler<CreatePasswordResetTokenCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly NotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CreatePasswordResetTokenCommandHandler> _logger;

        public CreatePasswordResetTokenCommandHandler(
            UserManager<User> userManager,
            NotificationService notificationService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<CreatePasswordResetTokenCommandHandler> logger)
        {
            _userManager = userManager;
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreatePasswordResetTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);

                if (user is null)
                {
                    return Unit.Value;
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                await this._notificationService.Send(
                    new Notification<PasswordResetToken>(
                        new(user.Email, token)
                    )
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to send password reset code");
            }

            return Unit.Value;
        }
    }
}
