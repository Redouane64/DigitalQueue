using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Options;

namespace DigitalQueue.Web.Services.MailService;

public class MailService
{
    private readonly ILogger<MailService> _logger;
    private readonly SmtpConfig _config;

    public MailService(IOptions<SmtpConfig> config, ILogger<MailService> logger)
    {
        _logger = logger;
        _config = config.Value;
    }

    public async Task SendEmailConfirmation(string to, string confirmationLink)
    {
        using var client = new SmtpClient(_config.Host, _config.Port)
        {
            Credentials = new NetworkCredential(_config.Username, _config.Password),
            EnableSsl = true,
            UseDefaultCredentials = false
        };

        var templateFile = this.GetType().Assembly.GetManifestResourceStream(
            "DigitalQueue.Web.Templates.ConfirmEmail_Template.html");

        if (templateFile is null)
        {
            return;
        }
        
        using var template = new StreamReader(templateFile);
        var body = await template.ReadToEndAsync();
        var regex = new Regex("{{confirmationLink}}");
        body = regex.Replace(body, confirmationLink);
        
        await client.SendMailAsync(
            new MailMessage(new MailAddress(_config.Username, _config.Name), new MailAddress(to))
            {
                IsBodyHtml = true,
                Subject = "Confirm Digital Queue Account", 
                Body = body
            }
        );
        
    }

    public async Task SendPasswordReset(string to, string code)
    {
        using var client = new SmtpClient(_config.Host, _config.Port)
        {
            Credentials = new NetworkCredential(_config.Username, _config.Password),
            EnableSsl = true,
            UseDefaultCredentials = false
        };

        var templateFile = this.GetType().Assembly.GetManifestResourceStream(
            "DigitalQueue.Web.Templates.PasswordReset_Template.html");

        if (templateFile is null)
        {
            return;
        }
        
        using var template = new StreamReader(templateFile);
        var body = await template.ReadToEndAsync();
        var regex = new Regex("{{code}}");
        body = regex.Replace(body, code);
        
        await client.SendMailAsync(
            new MailMessage(new MailAddress(_config.Username, _config.Name), new MailAddress(to))
            {
                IsBodyHtml = true,
                Subject = "Reset Password For Digital Queue Account", 
                Body = body
            }
        );
    }
}
