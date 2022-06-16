using DigitalQueue.Web.Areas.Accounts.Commands.Account;
using DigitalQueue.Web.Areas.Accounts.Models;
using DigitalQueue.Web.Infrastructure;

using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Events.Handlers;

public class SendAuthenticationTokenHandler : INotificationHandler<UserAuthenticatedEvent>
{
    private readonly IMediator _mediator;

    public SendAuthenticationTokenHandler(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Handle(UserAuthenticatedEvent notification, CancellationToken cancellationToken)
    {
        _ = Enum.TryParse<AuthenticatedUserType>(
            notification.Data?.GetValueOrDefault("AuthenticatedUserType"), out var type);
        
        var transport = type == AuthenticatedUserType.Created ? "Email" : "PushNotification";
        var deviceToken = notification.Data?.GetValueOrDefault("DeviceToken");
        
        await _mediator.Send(
            new CreateUserTokenCommand(
                notification.User,
                UserTokenProvider.AuthenticationPurposeName,
                transport,
                deviceToken
            ), cancellationToken);
    }
}
