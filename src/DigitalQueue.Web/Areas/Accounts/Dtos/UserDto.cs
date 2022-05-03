using DigitalQueue.Web.Areas.Sessions.Dtos;
using DigitalQueue.Web.Data.Entities;

namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class UserDto
{
    public UserDto(User user, IEnumerable<AccountRoleDto> roles, IEnumerable<UserCourseRolesDto> permissions)
    {
        this.Id = user.Id;
        this.Email = user.Email;
        this.Name = user.Name;
        this.CreatedAt = user.CreateAt;
        this.Roles = roles;
        this.Permissions = permissions;
    }

    public string Id { get; }
    public string Email { get; }
    public string Name { get; }
    public DateTime CreatedAt { get; }
    public IEnumerable<AccountRoleDto> Roles { get; }
    public IEnumerable<UserCourseRolesDto> Permissions { get; }
    public IEnumerable<SessionDto> Sessions { get; set; }
}
