namespace DigitalQueue.Web.Areas.Courses.Dtos;

public class CourseRequest
{
    public string RequestId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string StudentId { get; set; }
    public string StudentName { get; set; }

    public string CourseId { get; set; }
    public string CourseTitle { get; set; }
    public int CourseYear { get; set; }
    
}
