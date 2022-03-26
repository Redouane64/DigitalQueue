using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Commands;

using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Events.Handlers;

public class EmailChangedEventHandler : INotificationHandler<EmailChangedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<AccountCreatedEventHandler> _logger;

    public EmailChangedEventHandler(
        IMediator mediator,
        ILogger<AccountCreatedEventHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    public async Task Handle(EmailChangedEvent notification, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CreateEmailConfirmationTokenCommand(new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, notification.Email),
            new Claim(ClaimTypes.NameIdentifier, notification.AccountId)
        })), CreateEmailConfirmationTokenCommand.ConfirmationMethod.Url), cancellationToken);
    }
    
}
