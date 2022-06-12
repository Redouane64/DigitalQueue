using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public partial class SendChangeEmailCodeCommand : IRequest
{
    public string Email { get; }

    public SendChangeEmailCodeCommand(string email)
    {
        Email = email;
    }
}
