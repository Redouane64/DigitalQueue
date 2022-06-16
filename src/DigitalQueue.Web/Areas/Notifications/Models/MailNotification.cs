using DigitalQueue.Web.Areas.Notifications.Services;

namespace DigitalQueue.Web.Areas.Notifications.Models;

public class MailNotification : PlatformNotification
{
    public string[] Recipients { get; }
    public string Subject { get; }
    public string Body { get; }

    public MailNotification(string[] recipients, string subject, string body)
    {
        Recipients = recipients;
        Subject = subject;
        Body = body;
    }

    public static async Task<MailNotification> CreateAuthenticationTokenNotification(string to, string token)
    {
        var body = await EmailTemplatesHelper.ParseTemplate(
            EmailTemplatesHelper.AuthenticationTokenTemplate,
            new("{{code}}", token)
        );

        return new MailNotification(new [] { to }, "Digital Queue Account Authentication Code", body);
    }
}
