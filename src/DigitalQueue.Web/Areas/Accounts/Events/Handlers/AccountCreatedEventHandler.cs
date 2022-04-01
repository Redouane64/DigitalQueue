using DigitalQueue.Web.Areas.Accounts.Commands;
using DigitalQueue.Web.Services.Notifications;

using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Events.Handlers;

public class AccountCreatedEventHandler : INotificationHandler<AccountCreatedEvent>
{
    private readonly IMediator _mediator;
    private readonly NotificationService _notificationService;
    private readonly ILogger<AccountCreatedEventHandler> _logger;

    public AccountCreatedEventHandler(
        IMediator mediator,
        NotificationService notificationService,
        ILogger<AccountCreatedEventHandler> logger)
    {
        _mediator = mediator;
        _notificationService = notificationService;
        _logger = logger;
    }
    
    public async Task Handle(AccountCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CreateEmailConfirmationTokenCommand(
            notification.AccountId, 
            CreateEmailConfirmationTokenCommand.ConfirmationMethod.Url), cancellationToken);
    }

}
