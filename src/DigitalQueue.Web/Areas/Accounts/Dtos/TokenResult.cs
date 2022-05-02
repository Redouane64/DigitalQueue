namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class TokenResult
{
    public TokenResult(string accessToken, string refreshToken)
    {
        this.AccessToken = accessToken;
        this.RefreshToken = refreshToken;
    }

    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}
