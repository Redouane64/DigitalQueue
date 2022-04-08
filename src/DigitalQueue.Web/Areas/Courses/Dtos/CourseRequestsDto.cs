namespace DigitalQueue.Web.Areas.Courses.Dtos;

public class CourseRequestsDto
{
    public CourseRequestsDto(string course, int total, IEnumerable<RequestInfoDto> requests)
    {
        Course = course;
        Total = total;
        Requests = requests;
    }

    public string Course { get; }
    public int Total { get; }
    public IEnumerable<RequestInfoDto> Requests { get; }
}
