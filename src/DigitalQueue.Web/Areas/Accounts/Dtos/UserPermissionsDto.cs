namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class UserPermissionsDto
{
    public IEnumerable<CourseRolesDto> Courses { get; }

    public UserPermissionsDto(IEnumerable<CourseRolesDto> courses)
    {
        Courses = courses;
    }
}
