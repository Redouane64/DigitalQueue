using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Options;

namespace DigitalQueue.Web.Services.Notifications;

public class MailService
{
    private readonly static string ResetPasswordCodeTemplate = "DigitalQueue.Web.Templates.PasswordReset_Template.html";

    private readonly static string AuthenticationCodeTemplate =
        "DigitalQueue.Web.Templates.AuthenticationCode_Template.html";

    private readonly ILogger<MailService> _logger;
    private readonly SmtpConfig _config;

    public MailService(IOptions<SmtpConfig> config, ILogger<MailService> logger)
    {
        _logger = logger;
        _config = config.Value;
    }

    public async Task SendAuthenticationCode(string to, string code)
    {
        if (to == null)
        {
            throw new ArgumentNullException(nameof(to));
        }

        if (code == null)
        {
            throw new ArgumentNullException(nameof(code));
        }


        var body = await this.ParseTemplate(
            AuthenticationCodeTemplate,
            new("{{code}}", code)
        );

        if (body is null)
        {
            return;
        }

        await Send(to, "Digital Queue Account Authentication Code", body);
    }

    public async Task SendPasswordResetCode(string to, string code)
    {
        if (to == null)
        {
            throw new ArgumentNullException(nameof(to));
        }

        if (code == null)
        {
            throw new ArgumentNullException(nameof(code));
        }

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
        try
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
                    IsBodyHtml = true, Subject = subject, Body = body
                }
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to send authentication code to user {to}", to);
            throw;
        }
    }

    private async Task<string?> ParseTemplate(string template, KeyValuePair<string, string> value)
    {
        var templateFile = this.GetType().Assembly.GetManifestResourceStream(template);

        if (templateFile is null)
        {
            _logger.LogError("E-mail template '{template}' not found", template);
            return null;
        }

        using var file = new StreamReader(templateFile);
        var body = await file.ReadToEndAsync();

        return Regex.Replace(body, value.Key, value.Value);
    }
}
