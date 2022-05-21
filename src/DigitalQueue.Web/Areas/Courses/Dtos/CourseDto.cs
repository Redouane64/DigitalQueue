namespace DigitalQueue.Web.Areas.Courses.Dtos;

public class CourseDto
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public int Year { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Teachers { get; set; }
    public int Students { get; set; }
    
}
