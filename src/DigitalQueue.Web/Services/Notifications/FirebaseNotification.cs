namespace DigitalQueue.Web.Services.Notifications;

public class FirebaseNotification : Notification<string>
{
    public string DeviceToken { get; }
    public string Title { get; }

    public FirebaseNotification(string deviceToken, string title, string body) 
        : base(body)
    {
        DeviceToken = deviceToken;
        Title = title;
    }
}
