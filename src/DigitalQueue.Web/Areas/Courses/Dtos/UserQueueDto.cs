namespace DigitalQueue.Web.Areas.Courses.Dtos;

public class UserQueueDto
{
    public UserQueueDto(string course, int total, IEnumerable<QueueItemDto> requests)
    {
        Course = course;
        Total = total;
        Requests = requests;
    }

    public string Course { get; }
    public int Total { get; }
    public IEnumerable<QueueItemDto> Requests { get; }
    
}
