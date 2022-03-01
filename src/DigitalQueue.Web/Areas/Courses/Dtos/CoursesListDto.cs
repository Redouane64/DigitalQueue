namespace DigitalQueue.Web.Areas.Courses.Dtos;

public class CoursesListDto
{
    public IEnumerable<CourseDto> Courses { get; }
    public int? Page { get; }
    public int? PageSize { get; }
    public bool FullView { get; }

    public CoursesListDto(IEnumerable<CourseDto> courses, int? page = null, int? pageSize = null, bool fullView = true)
    {
        Courses = courses;
        Page = page;
        PageSize = pageSize;
        FullView = fullView;
    }
}
