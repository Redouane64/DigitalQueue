namespace DigitalQueue.Web.Services.Notifications;

public static class ServiceCollectionExtensions 
{
    public static void AddNotificationService(this IServiceCollection service, IConfiguration configuration)
    {
        service.Configure<SmtpConfig>(configuration.GetSection("SmtpConfig"));
        service.AddSingleton<MailService>();
        service.AddSingleton<NotificationService>();
    }
}
