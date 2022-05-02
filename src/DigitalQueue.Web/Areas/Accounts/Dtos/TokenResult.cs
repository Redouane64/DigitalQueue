namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class TokenResult
{
    public TokenResult(string accessToken, string refreshToken, DateTime expires)
    {
        this.AccessToken = accessToken;
        this.RefreshToken = refreshToken;
        this.Expires = expires;
    }

    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
    public DateTime Expires { get; init; }

    public void Deconstruct(out string accessToken, out string refreshToken, out DateTime expires)
    {
        accessToken = this.AccessToken;
        refreshToken = this.RefreshToken;
        expires = this.Expires;
    }
}
