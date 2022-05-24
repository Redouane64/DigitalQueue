using FirebaseAdmin;

using Google.Apis.Auth.OAuth2;

namespace DigitalQueue.Web.Services.Notifications;

public static class ServiceCollectionExtensions 
{
    public static void AddNotificationService(this IServiceCollection service, IConfiguration configuration)
    {
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromServiceAccountCredential(
                ServiceAccountCredential.FromServiceAccountData(File.OpenRead("service_account.json"))
            )
        });

        service.Configure<SmtpConfig>(configuration.GetSection("SmtpConfig"));
        service.AddSingleton<MailService>();
        service.AddSingleton<FirebaseNotificationService>();
        
        service.AddSingleton<NotificationService>();
    }
}
