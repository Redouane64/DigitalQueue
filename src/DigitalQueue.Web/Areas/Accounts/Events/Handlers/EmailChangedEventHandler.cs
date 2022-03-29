using DigitalQueue.Web.Areas.Accounts.Commands;

using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Events.Handlers;

public class EmailChangedEventHandler : INotificationHandler<EmailChangedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<EmailChangedEventHandler> _logger;

    public EmailChangedEventHandler(
        IMediator mediator,
        ILogger<EmailChangedEventHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    public async Task Handle(EmailChangedEvent notification, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CreateEmailConfirmationTokenCommand(
            notification.AccountId, 
            CreateEmailConfirmationTokenCommand.ConfirmationMethod.Url), cancellationToken);
    }
    
}
