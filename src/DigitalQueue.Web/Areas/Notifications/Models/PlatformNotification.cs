namespace DigitalQueue.Web.Areas.Notifications.Models;

public abstract class PlatformNotification
{
    public string[] To { get; protected set; }
    public string Subject { get; protected set; }
    public string Body { get; protected set; }
}
