using DigitalQueue.Web.Areas.Courses.Dtos;

namespace DigitalQueue.Web.Areas.Accounts.Dtos;

public record CourseRolesDto
{
    public CourseRolesDto(string CourseId, string[] Roles)
    {
        this.CourseId = CourseId;
        this.Roles = Roles;
    }
    
    public CourseRolesDto(CourseDto course, string[] roles)
    {
        this.Course = course;
        this.CourseId = course.Id;
        this.Roles = roles;
    }
    
    public string CourseId { get; }
    public CourseDto Course { get; }
    public string[] Roles { get; }

    public void Deconstruct(out CourseDto course, out string[] roles)
    {
        course = this.Course;
        roles = this.Roles;
    }
}
