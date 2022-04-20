using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Services.Notifications;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class CreatePasswordResetTokenCommand : IRequest
{
    public string UserId { get; }

    public CreatePasswordResetTokenCommand(string userId)
    {
        UserId = userId;
    }

    public class CreatePasswordResetTokenCommandHandler : IRequestHandler<CreatePasswordResetTokenCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly NotificationService _notificationService;
        private readonly ILogger<CreatePasswordResetTokenCommandHandler> _logger;

        public CreatePasswordResetTokenCommandHandler(
            UserManager<User> userManager,
            NotificationService notificationService,
            ILogger<CreatePasswordResetTokenCommandHandler> logger)
        {
            _userManager = userManager;
            _notificationService = notificationService;
            _logger = logger;
        }
        
        public async Task<Unit> Handle(CreatePasswordResetTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
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
