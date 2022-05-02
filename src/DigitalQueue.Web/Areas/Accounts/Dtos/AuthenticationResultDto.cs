namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class AuthenticationResultDto : TokenResult
{
    public string SessionId { get; }

    public AuthenticationResultDto(string sessionId, string accessToken, string refreshToken, DateTime expires) 
        : base(accessToken, refreshToken, expires)
    {
        SessionId = sessionId;
    }
}
