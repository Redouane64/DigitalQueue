using System.Text.Json.Serialization;

namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class AuthenticationResultDto : TokenResult
{
    [JsonIgnore]
    public string Session { get; }

    public AuthenticationResultDto(string session, string accessToken, string refreshToken) 
        : base(accessToken, refreshToken)
    {
        Session = session;
    }
}

public class AuthenticationStatusDto
{
    public bool Created { get; set; }
}
