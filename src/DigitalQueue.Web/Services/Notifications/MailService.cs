using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Options;

namespace DigitalQueue.Web.Services.Notifications;

public class MailService
{
    private readonly static string ResetPasswordCodeTemplate = "DigitalQueue.Web.Templates.PasswordReset_Template.html";
    private readonly static string ConfirmEmailUrlTemplate = "DigitalQueue.Web.Templates.ConfirmEmail_Template.html";
    private readonly static string ConfirmEmailCodeTemplate = "DigitalQueue.Web.Templates.ConfirmEmailWithCode_Template.html";
    
    private readonly ILogger<MailService> _logger;
    private readonly SmtpConfig _config;

    public MailService(IOptions<SmtpConfig> config, ILogger<MailService> logger)
    {
        _logger = logger;
        _config = config.Value;
    }

    public async Task SendEmailConfirmationUrl(string to, string confirmationLink)
    {
        var body = await this.ParseTemplate(
            ConfirmEmailUrlTemplate, 
            new("{{confirmationLink}}", confirmationLink)
        );
        
        if (body is null)
        {
            return;
        }
        
        await Send(to, "Confirm Digital Queue Account", body);
    }

    public async Task SendEmailConfirmationCode(string to, string code)
    {
        var body = await this.ParseTemplate(
            ConfirmEmailCodeTemplate, 
            new("{{code}}", code)
        );
        
        if (body is null)
        {
            return;
        }
        
        await Send(to, "Confirm Digital Queue Account", body);
    }
    
    public async Task SendPasswordResetCode(string to, string code)
    {
        var body = await this.ParseTemplate(
            ResetPasswordCodeTemplate, 
            new("{{code}}", code)
        );
        
        if (body is null)
        {
            return;
        }
        
        await Send(to, "Password Reset Code For Digital Queue Account", body);
    }

    private async Task Send(string to, string subject, string body)
    {
        using var client = new SmtpClient(_config.Host, _config.Port)
        {
            Credentials = new NetworkCredential(_config.Username, _config.Password),
            EnableSsl = true,
            UseDefaultCredentials = false
        };

        await client.SendMailAsync(
            new MailMessage(new MailAddress(_config.Username, _config.Name), new MailAddress(to))
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = body
            }
        );
    }

    private async Task<string?> ParseTemplate(string template, KeyValuePair<string, string> value)
    {
        var templateFile = this.GetType().Assembly.GetManifestResourceStream(template);

        if (templateFile is null)
        {
            return null;
        }

        using var file = new StreamReader(templateFile);
        var body = await file.ReadToEndAsync();

        return Regex.Replace(body, value.Key, value.Value);
    }
    
}
