using DigitalQueue.Web.Areas.Courses.Dtos;

namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public class UserCourseRolesDto
{
    public UserCourseRolesDto(string courseId, string[] roles)
    {
        CourseId = courseId;
        Roles = roles;
    }

    public UserCourseRolesDto(string courseId, string courseTitle, string[] roles)
    {
        CourseId = courseId;
        CourseTitle = courseTitle;
        Roles = roles;
    }

    public string CourseId { get; }

    public string CourseTitle { get; }

    public string[] Roles { get; }
    
}
