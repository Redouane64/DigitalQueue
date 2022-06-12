using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public partial class UpdateEmailCommand : IRequest<bool>
{
    public string Token { get; }
    public string Email { get; }

    public UpdateEmailCommand(string token, string email)
    {
        Token = token;
        Email = email;
    }
}
