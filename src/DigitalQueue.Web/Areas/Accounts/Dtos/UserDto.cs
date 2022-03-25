using DigitalQueue.Web.Data.Entities;

namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class UserDto
{
    public UserDto(User user, IEnumerable<AccountRoleDto> accountRoles, IEnumerable<UserCourseRolesDto> coursesRoles)
    {
        this.Id = user.Id;
        this.Email = user.Email;
        this.Name = user.Name;
        this.CreatedAt = user.CreateAt;
        this.AccountRoles = accountRoles;
        this.CoursesRoles = coursesRoles;
    }

    public string Id { get; }
    public string Email { get; }
    public string Name { get; }
    public DateTime CreatedAt { get; }
    public IEnumerable<AccountRoleDto> AccountRoles { get; }
    public IEnumerable<UserCourseRolesDto> CoursesRoles { get; }
    
}
