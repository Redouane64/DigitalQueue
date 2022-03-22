using DigitalQueue.Web.Areas.Accounts.Events.Dtos;

using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Events;

public class AccountCreatedEvent : INotification, IAccountEventDto
{
    public string AccountId { get; }
    public string Email { get; }

    public AccountCreatedEvent(string accountId, string email)
    {
        AccountId = accountId;
        Email = email;
    }
}
