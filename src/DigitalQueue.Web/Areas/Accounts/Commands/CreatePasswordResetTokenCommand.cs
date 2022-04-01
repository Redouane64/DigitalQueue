using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Services.Notifications;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public class CreatePasswordResetTokenCommand : IRequest<bool>
{
    public string UserId { get; }

    public CreatePasswordResetTokenCommand(string userId)
    {
        UserId = userId;
    }

    public class CreatePasswordResetTokenCommandHandler : IRequestHandler<CreatePasswordResetTokenCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly NotificationService _notificationService;

        public CreatePasswordResetTokenCommandHandler(
            UserManager<User> userManager,
            NotificationService notificationService)
        {
            _userManager = userManager;
            _notificationService = notificationService;
        }
        
        public async Task<bool> Handle(CreatePasswordResetTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                await this._notificationService.Publish(
                    new Notification<SendPasswordResetCode>(
                        new(user.Email, token)
                    )
                );
            }
            catch (Exception)
            {
                return false;
            }
            
            return true;
        }
    }
}
