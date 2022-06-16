using DigitalQueue.Web.Areas.Notifications.Models;

namespace DigitalQueue.Web.Areas.Notifications.Services;

public interface INotificationService<in TNotification> where TNotification : PlatformNotification
{
    Task Send(TNotification notification);
}
