namespace DigitalQueue.Web.Services.MailService;

public static class MailServiceExtensions 
{
    public static void AddMailService(this IServiceCollection service, IConfiguration configuration)
    {
        service.Configure<SmtpConfig>(configuration.GetSection("SmtpConfig"));
        service.AddTransient<MailService>();
    }
}
