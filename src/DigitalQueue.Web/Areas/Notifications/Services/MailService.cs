using System.Net;
using System.Net.Mail;

using DigitalQueue.Web.Areas.Notifications.Extensions;
using DigitalQueue.Web.Areas.Notifications.Models;

using Microsoft.Extensions.Options;

namespace DigitalQueue.Web.Areas.Notifications.Services;

public class MailService : INotificationService<MailNotification>
{
    private readonly ILogger<MailService> _logger;
    private readonly SmtpConfig _config;

    public MailService(IOptions<SmtpConfig> config, ILogger<MailService> logger)
    {
        _logger = logger;
        _config = config.Value;
    }
    
    public async Task Send(MailNotification platformNotification)
    {
        using var client = new SmtpClient(_config.Host, _config.Port)
        {
            Credentials = new NetworkCredential(_config.Username, _config.Password),
            EnableSsl = true,
            UseDefaultCredentials = false
        };

        var message = new MailMessage();
        message.From = new MailAddress(_config.Username!, _config.Name);
        foreach (var email in platformNotification.Recipients)
            message.To.Add(email);
        
        message.IsBodyHtml = true;
        message.Subject = platformNotification.Subject;
        message.Body = platformNotification.Body;

        await client.SendMailAsync(message);
    }
    
}
