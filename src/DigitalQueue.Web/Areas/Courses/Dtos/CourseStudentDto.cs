namespace DigitalQueue.Web.Areas.Courses.Dtos;

public class CourseStudentDto
{
    public string Name { get; }
    public string Id { get; }

    public CourseStudentDto(string name, string id)
    {
        Name = name;
        Id = id;
    }
}
