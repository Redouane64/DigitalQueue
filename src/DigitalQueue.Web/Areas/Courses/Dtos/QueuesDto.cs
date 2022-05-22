namespace DigitalQueue.Web.Areas.Courses.Dtos;

public class QueuesDto
{
    public IEnumerable<CourseQueueDto> Sent { get; }
    public IEnumerable<CourseQueueDto> Received { get; }

    public QueuesDto(IEnumerable<CourseQueueDto> sent, IEnumerable<CourseQueueDto> received)
    {
        Sent = sent;
        Received = received;
    }
}
