using MediatR;

namespace DigitalQueue.Web.Areas.Accounts.Commands;

public partial class UpdatePasswordCommand : IRequest<bool>
{

    public UpdatePasswordCommand(string password, string token)
    {
        Password = password;
        Token = token;
    }

    public string Password { get; }
    public string Token { get; }
}
