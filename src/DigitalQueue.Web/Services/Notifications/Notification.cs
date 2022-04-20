using DigitalQueue.Web.Areas.Accounts.Commands;

namespace DigitalQueue.Web.Services.Notifications;

public class Notification<T> where T : class
{
    public Notification(T payload)
    {
        Payload = payload;
    }

    public T Payload { get; }
}

public record EmailConfirmationToken(
    string Email, 
    CreateEmailConfirmationTokenCommand.ConfirmationMethod Method,
    string Token
);

public record PasswordResetToken(string Email, string Token);
