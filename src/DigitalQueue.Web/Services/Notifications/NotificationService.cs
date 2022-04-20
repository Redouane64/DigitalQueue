using DigitalQueue.Web.Areas.Accounts.Commands;

namespace DigitalQueue.Web.Services.Notifications;

public class NotificationService
{
    private readonly string ChannelName = "Notifications";
    private readonly MailService _mailService;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        MailService mailService,
        ILogger<NotificationService> logger)
    {
        _mailService = mailService;
        _logger = logger;
    }
    
    public async Task Send<T>(Notification<T> notification) where T : class
    {
        try
        {
            if (notification.Payload is EmailConfirmationToken emailConfirmationToken)
            {
                switch (emailConfirmationToken.Method)
                {
                    case CreateEmailConfirmationTokenCommand.ConfirmationMethod.Url:
                        await this._mailService.SendEmailConfirmationUrl(emailConfirmationToken.Email, emailConfirmationToken.Token);
                        break;
                    case CreateEmailConfirmationTokenCommand.ConfirmationMethod.Code:
                        await this._mailService.SendEmailConfirmationCode(emailConfirmationToken.Email, emailConfirmationToken.Token);
                        break;
                }
                return;
            }
            
            if (notification.Payload is PasswordResetToken passwordResetToken)
            {
                await this._mailService.SendPasswordResetCode(passwordResetToken.Email, passwordResetToken.Token);
                return;
            }
            
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to publish payload");
        }
    }

}
