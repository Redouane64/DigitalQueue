namespace DigitalQueue.Web.Users;

public class JwtOptions
{
    public string? Secret { get; set; }

    public string? Issuer { get; set; }

    public string? Audience { get; set; }

    public int TokenLifeTime { get; set; }

    public int RefreshTokenLifeTime { get; set; }
}
