using System.Text.RegularExpressions;

namespace DigitalQueue.Web.Areas.Notifications.Services;

public static class EmailTemplatesHelper
{
    
    public readonly static string AuthenticationTokenTemplate =
        "DigitalQueue.Web.Templates.AuthenticationCode_Template.html";
    
    public static async Task<string> ParseTemplate(string templateName, KeyValuePair<string, string> value)
    {
        var template = typeof(EmailTemplatesHelper).Assembly.GetManifestResourceStream(templateName);

        using var file = new StreamReader(template!);
        var body = await file.ReadToEndAsync();

        return Regex.Replace(body, value.Key, value.Value);
    }
}
