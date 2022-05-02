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
        switch (notification.Payload)
        {
            case VerificationToken verificationToken:
                await this._mailService.SendEmailConfirmationCode(verificationToken.Email, verificationToken.Token);
                return;
            case PasswordResetToken passwordResetToken:
                await this._mailService.SendPasswordResetCode(passwordResetToken.Email, passwordResetToken.Token);
                break;
        }
    }
}
