using DigitalQueue.Web.Areas.Accounts.Commands;

namespace DigitalQueue.Web.Services.Notifications;

public class Notification<T> where T : class
{
    public Notification(T payload)
    {
        Type = typeof(T).Name;
        Payload = payload;
    }

    public string Type { get; }
    public T Payload { get; }
}

public record SendEmailConfirmation(
    string Email, 
    CreateEmailConfirmationTokenCommand.ConfirmationMethod Method,
    string Token
);

public record SendPasswordResetCode(string Email, string Token);
