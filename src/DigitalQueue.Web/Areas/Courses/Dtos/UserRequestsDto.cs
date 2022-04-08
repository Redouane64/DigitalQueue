namespace DigitalQueue.Web.Areas.Courses.Dtos;

public class UserRequestsDto
{
    public UserRequestsDto(string course, int total, IEnumerable<RequestInfoDto> requests)
    {
        Course = course;
        Total = total;
        Requests = requests;
    }

    public string Course { get; }
    public int Total { get; }
    public IEnumerable<RequestInfoDto> Requests { get; }
    
}
