using DigitalQueue.Web.Areas.Accounts.Events.Dtos;

using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Events;

public class EmailChangedEvent : INotification, IAccountEventDto
{
    public string AccountId { get; }
    public string Email { get; }
    
    public EmailChangedEvent(string accountId, string email)
    {
        AccountId = accountId;
        Email = email;
    }

}
