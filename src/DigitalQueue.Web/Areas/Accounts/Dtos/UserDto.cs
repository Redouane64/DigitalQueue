using DigitalQueue.Web.Data.Entities;

namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class UserDto
{
    public UserDto(User user, IEnumerable<RoleDto> roles, IEnumerable<ClaimDto> claims)
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
    public IEnumerable<RoleDto> Roles { get; }
    public IEnumerable<ClaimDto> Claims { get; }
    
}
