using System.Security.Claims;

using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Events;

public class AccountCreatedEvent : INotification
{
    public ClaimsPrincipal? Principal { get; }
    public string? Id { get; }
    public string? Email { get; }

    public AccountCreatedEvent(string id, string email)
    {
        Id = id;
        Email = email;
    }

}
