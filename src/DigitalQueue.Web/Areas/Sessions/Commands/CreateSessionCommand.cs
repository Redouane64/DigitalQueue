using MediatR;

namespace DigitalQueue.Web.Areas.Sessions.Commands;

public partial class CreateSessionCommand : IRequest
{
    public string UserId { get; }
    public string SecurityStamp { get; }
    public string AccessToken { get; }
    public string RefreshToken { get; }

    public CreateSessionCommand(string userId, string securityStamp, string accessToken, string refreshToken)
    {
        UserId = userId;
        SecurityStamp = securityStamp;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}
