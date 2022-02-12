using DigitalQueue.Web.Data.Entities;

namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public record UserClaimDto(string Claim, string[] Values);

public class UserDto
{
    public UserDto(User user, IEnumerable<string> roles, IEnumerable<UserClaimDto> claims)
    {
        this.Id = user.Id;
        this.Email = user.Email;
        this.Username = user.UserName;
        this.Roles = roles;
        this.Claims = claims;
    }

    public string Id { get; }
    public string Email { get; }
    public string Username { get; }
    public IEnumerable<string> Roles { get; }
    public IEnumerable<UserClaimDto> Claims { get; }
    
}
