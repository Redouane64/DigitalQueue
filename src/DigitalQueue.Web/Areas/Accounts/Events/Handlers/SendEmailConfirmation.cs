using System.Text;

using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Services.MailService;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace DigitalQueue.Web.Areas.Accounts.Events.Handlers;

public class SendEmailConfirmation : INotificationHandler<AccountCreatedEvent>
{
    private readonly UserManager<User> _userManager;
    private readonly MailService _mailService;
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<SendEmailConfirmation> _logger;

    public SendEmailConfirmation(
        UserManager<User> userManager,
        MailService mailService, 
        LinkGenerator linkGenerator, 
        IHttpContextAccessor httpContextAccessor,
        ILogger<SendEmailConfirmation> logger)
    {
        _userManager = userManager;
        _mailService = mailService;
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }
    
    public async Task Handle(AccountCreatedEvent notification, CancellationToken cancellationToken)
    {
        // TODO: enqueue this message to a message broker or pub-sub
        // for asynchronous processing because SMTP execution is slow
        try
        {
            User user = null;
            if (notification.Principal is not null)
            {
                user = await this._userManager.GetUserAsync(notification.Principal);
            }
            else
            {
                user = new User {Id = notification.Id, Email = notification.Email};
            }

            var token = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(await _userManager.GenerateEmailConfirmationTokenAsync(user)));
        
            var confirmationLink =
                this._linkGenerator.GetUriByPage(_httpContextAccessor.HttpContext, 
                    "/ConfirmEmail", 
                    null, 
                    new { token, email = user.Email, area = "Accounts" });

            await this._mailService.SendEmailConfirmation(notification.Email, confirmationLink);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to send e-mail confirmation");
        }
    }
}
