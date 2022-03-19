using DigitalQueue.Web.Data.Entities;

namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class UserDto
{
    public UserDto(User user, IEnumerable<AccountRoleDto> accountRoles, IEnumerable<UserCourseRolesDto> coursesRoles)
    {
        this.Id = user.Id;
        this.Email = user.Email;
        this.Username = user.UserName;
        this.FullName = user.FullName;
        this.CreatedAt = user.CreateAt;
        this.AccountRoles = accountRoles;
        this.CoursesRoles = coursesRoles;
    }

    public string Id { get; }
    public string Email { get; }
    public string Username { get; }
    public string FullName { get; }
    public DateTime CreatedAt { get; }
    public IEnumerable<AccountRoleDto> AccountRoles { get; }
    public IEnumerable<UserCourseRolesDto> CoursesRoles { get; }
    
}
