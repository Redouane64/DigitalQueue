using DigitalQueue.Web.Areas.Accounts.Dtos;

using MediatR;

namespace DigitalQueue.Web.Areas.Sessions.Commands;

public partial class RefreshSessionCommand : IRequest<TokenResult?>
{
    public string RefreshToken { get; }

    public RefreshSessionCommand(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
}
