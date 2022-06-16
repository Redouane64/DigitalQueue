using DigitalQueue.Web.Areas.Notifications.Models;

using FirebaseAdmin.Messaging;

namespace DigitalQueue.Web.Areas.Notifications.Services;

public class FirebaseService : INotificationService<FirebaseNotification>
{
    private readonly ILogger<FirebaseService> _logger;

    public FirebaseService(ILogger<FirebaseService> logger)
    {
        _logger = logger;
    }

    public async Task Send(FirebaseNotification platformNotification)
    {
        var message = new MulticastMessage()
        {
            Tokens = platformNotification.To,
        };

        if (platformNotification.Body is not null)
        {
            message.Notification = new Notification
            {
                Body = platformNotification.Body, 
                Title = platformNotification.Subject
            };
        }

        if (platformNotification.Data is not null)
        {
            message.Data = platformNotification.Data;
        }
        
        _ = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
    }

}
