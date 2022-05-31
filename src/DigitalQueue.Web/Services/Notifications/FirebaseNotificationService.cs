using FirebaseAdmin.Messaging;

namespace DigitalQueue.Web.Services.Notifications;

public class FirebaseNotificationService
{
    private readonly ILogger<FirebaseNotificationService> _logger;

    public FirebaseNotificationService(ILogger<FirebaseNotificationService> logger)
    {
        _logger = logger;
    }

    public async Task SendData(IReadOnlyList<string> tokens, IReadOnlyDictionary<string, string> data)
    {
        if (tokens.Count == 0 || data.Count == 0)
        {
            return;
        }

        var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(
            new MulticastMessage { Tokens = tokens, Data = data });
        
        _logger.LogInformation("{} sent notification, {} failed notification", response.SuccessCount, response.FailureCount);
    }

    public async Task Send(FirebaseNotification notification)
    {
        if (notification.Tokens.Length == 0)
        {
            return;
        }

        var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(
            new MulticastMessage()
            {
                Tokens = notification.Tokens,

                Notification = new Notification()
                {
                    Body = notification.Payload,
                    Title = notification.Title
                }
            }
        );
        _logger.LogInformation("{} sent notification, {} failed notification", response.SuccessCount, response.FailureCount);
    }

}
