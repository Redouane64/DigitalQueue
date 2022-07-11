namespace DigitalQueue.Web.Areas.Courses.Models;

public class CourseQueueItem
{
    public string ItemId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Student { get; set; }
    public string Course { get; set; }
}
