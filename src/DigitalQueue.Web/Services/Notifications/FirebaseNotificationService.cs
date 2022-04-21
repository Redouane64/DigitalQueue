using FirebaseAdmin.Messaging;

namespace DigitalQueue.Web.Services.Notifications;

public class FirebaseNotificationService
{
    private readonly ILogger<FirebaseNotificationService> _logger;

    public FirebaseNotificationService(ILogger<FirebaseNotificationService> logger)
    {
        _logger = logger;
    }
    
    public async Task Send(FirebaseNotification notification)
    {
        try
        {
            string messageId = await FirebaseMessaging.DefaultInstance.SendAsync(new Message()
            {
                Notification = new Notification
                {
                    Body = notification.Payload, 
                    Title = notification.Title
                }, 
                Token = notification.DeviceToken
            });
            
            _logger.LogInformation("Message {messageId} sent", messageId);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unable to send firebase message");
        }
    }

}
