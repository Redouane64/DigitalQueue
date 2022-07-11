namespace DigitalQueue.Web.Areas.Courses.Models;

public class CoursesListDto
{
    public IEnumerable<CourseResult> Courses { get; }
    public int? Page { get; }
    public int? PageSize { get; }
    public bool FullView { get; }

    public CoursesListDto(IEnumerable<CourseResult> courses, int? page = null, int? pageSize = null, bool fullView = true)
    {
        Courses = courses;
        Page = page;
        PageSize = pageSize;
        FullView = fullView;
    }
}
