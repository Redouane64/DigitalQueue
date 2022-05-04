namespace DigitalQueue.Web.Areas.Courses.Dtos;

public class QueueDto
{
    public QueueDto(UserQueueDto[] sent, CourseQueueDto[] received)
    {
        Sent = sent;
        Received = received;
    }

    public UserQueueDto[] Sent { get; }
    public CourseQueueDto[] Received { get; }
    
}
