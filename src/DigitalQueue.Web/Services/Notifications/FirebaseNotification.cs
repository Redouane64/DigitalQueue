namespace DigitalQueue.Web.Services.Notifications;

public class FirebaseNotification : Notification<string>
{
    public string[] Tokens { get; }
    public string Title { get; }

    public FirebaseNotification(string[] tokens, string title, string body)
        : base(body)
    {
        Tokens = tokens;
        Title = title;
    }
}
