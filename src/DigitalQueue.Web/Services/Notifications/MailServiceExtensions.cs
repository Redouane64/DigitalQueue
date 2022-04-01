using StackExchange.Redis;

namespace DigitalQueue.Web.Services.Notifications;

public static class MailServiceExtensions 
{
    public static void AddNotificationService(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddSingleton<IConnectionMultiplexer>((provider) =>
        {
            var server = configuration.GetValue<string>("Redis:Server");
            return ConnectionMultiplexer.Connect(server);
        });
        
        service.Configure<SmtpConfig>(configuration.GetSection("SmtpConfig"));
        service.AddSingleton<MailService>();
        
        service.AddSingleton<NotificationService>();
    }
}
