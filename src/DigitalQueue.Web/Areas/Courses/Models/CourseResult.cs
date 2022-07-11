namespace DigitalQueue.Web.Areas.Courses.Models;

public class CourseResult
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public int? Year { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int Teachers { get; set; }
    public int Students { get; set; }

}
