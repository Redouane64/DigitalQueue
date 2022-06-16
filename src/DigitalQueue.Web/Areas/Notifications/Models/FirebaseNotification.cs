namespace DigitalQueue.Web.Areas.Notifications.Models;

public class FirebaseNotification : PlatformNotification
{
    public IReadOnlyDictionary<string, string>? Data { get; }

    public FirebaseNotification(string[] to, string subject, string body)
    {
        To = to;
        Subject = subject;
        Body = body;
    }

    public FirebaseNotification(string[] to, IReadOnlyDictionary<string, string> data)
    {
        To = to;
        Data = data;
    }
}
