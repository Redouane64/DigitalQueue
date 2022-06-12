using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Infrastructure;
using DigitalQueue.Web.Services.Notifications;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public partial class SendChangeEmailCodeCommand
{
    public class SendChangeEmailCodeCommandHandler : IRequestHandler<SendChangeEmailCodeCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly NotificationService _notificationService;
        private readonly ILogger<SendChangeEmailCodeCommandHandler> _logger;

        public SendChangeEmailCodeCommandHandler(
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            NotificationService notificationService,
            ILogger<SendChangeEmailCodeCommandHandler> logger)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<Unit> Handle(SendChangeEmailCodeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
            if (user is null)
            {
                _logger.LogWarning("User not found");
                return Unit.Value;
            }

            var token = await _userManager.GenerateUserTokenAsync(user, AuthenticationTokenProvider.ProviderName,
                AuthenticationTokenProvider.AuthenticationPurposeName);

            try
            {
                await _notificationService.Send(new Notification<VerificationToken>(
                    new VerificationToken(request.Email, token)));
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Unable to send token");
            }

            return Unit.Value;
        }
    }
}
