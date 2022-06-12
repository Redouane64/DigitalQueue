using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public partial class CreateDefaultAccountCommand : IRequest
{
    public string Email { get; }
    public string Password { get; }

    public CreateDefaultAccountCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
