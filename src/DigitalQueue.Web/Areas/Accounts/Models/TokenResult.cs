namespace DigitalQueue.Web.Areas.Accounts.Models;

public class TokenResult
{
    public TokenResult(string accessToken, string refreshToken)
    {
        this.AccessToken = accessToken;
        this.RefreshToken = refreshToken;
    }

    public string AccessToken { get; }
    public string RefreshToken { get; }
}
