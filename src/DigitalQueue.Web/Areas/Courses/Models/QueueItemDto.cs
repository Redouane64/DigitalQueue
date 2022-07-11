namespace DigitalQueue.Web.Areas.Courses.Models;

public class QueueItemDto
{
    public string Id { get; set; }
    public string Student { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public bool You { get; set; }
    
}
