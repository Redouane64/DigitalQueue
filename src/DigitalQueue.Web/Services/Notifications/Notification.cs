namespace DigitalQueue.Web.Services.Notifications;

public class Notification<T> where T : class
{
    public Notification(T payload)
    {
        Payload = payload;
    }

    public T Payload { get; }
}
