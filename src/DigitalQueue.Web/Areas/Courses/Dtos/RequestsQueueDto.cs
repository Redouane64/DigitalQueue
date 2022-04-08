namespace DigitalQueue.Web.Areas.Courses.Dtos;

public class RequestsQueueDto
{
    public RequestsQueueDto(UserRequestsDto[] sent, CourseRequestsDto[] received)
    {
        Sent = sent;
        Received = received;
    }

    public UserRequestsDto[] Sent { get; }
    public CourseRequestsDto[] Received { get; }
    
}
