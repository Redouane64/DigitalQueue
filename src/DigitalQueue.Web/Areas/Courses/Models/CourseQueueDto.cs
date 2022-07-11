namespace DigitalQueue.Web.Areas.Courses.Models;

public class CourseQueueDto
{
    public CourseQueueDto(string course, string courseId, int total)
    {
        Course = course;
        CourseId = courseId;
        Total = total;
    }

    public string Course { get; }
    public string CourseId { get; }
    public int Total { get; }
}
