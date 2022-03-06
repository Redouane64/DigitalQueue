namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class UserPermissionsDto
{
    public IEnumerable<UserCourseRolesDto> Courses { get; }

    public UserPermissionsDto(IEnumerable<UserCourseRolesDto> courses)
    {
        Courses = courses;
    }
}
