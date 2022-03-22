using DigitalQueue.Web.Areas.Accounts.Events.Dtos;
using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Services.MailService;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Areas.Accounts.Events.Handlers;

public class SendEmailConfirmation : INotificationHandler<AccountCreatedEvent>, INotificationHandler<EmailChangedEvent>
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
        await SendEmailNotification(notification);
    }
    
    public async Task Handle(EmailChangedEvent notification, CancellationToken cancellationToken)
    {
        await SendEmailNotification(notification);
    }
    
    private async Task SendEmailNotification(IAccountEventDto data)
    {
        // TODO: enqueue this message to a message broker or pub-sub
        // for asynchronous processing because SMTP execution is slow
        try
        {
            User user = new User { Id = data.AccountId, Email = data.Email };

            string? token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            string? confirmationLink =
                _linkGenerator.GetUriByPage(_httpContextAccessor.HttpContext!,
                    "/ConfirmEmail",
                    null,
                    new {token, email = user.Email, area = "Accounts"});

            if (confirmationLink is null)
            {
                _logger.LogWarning("Confirmation URL were not generated");
                return;
            }

            await _mailService.SendEmailConfirmation(data.Email, confirmationLink);
            _logger.LogInformation("E-mail confirmation notification sent to {Email}", user.Email);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to send e-mail confirmation notification");
        }
    }

}
