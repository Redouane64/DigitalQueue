namespace DigitalQueue.Web.Areas.Courses.Dtos;

public record CourseDto
{
    public CourseDto(string id, string title)
    {
        this.Id = id;
        this.Title = title;
    }
    
    public CourseDto(string id, string title, int teachersCount, int studentsCount)
    {
        this.Id = id ?? throw new ArgumentNullException(nameof(id));
        this.Title = title ?? throw new ArgumentNullException(nameof(title));
        this.TeachersCount = teachersCount;
        this.StudentsCount = studentsCount;
    }

    public string Id { get; init; }
    public string Title { get; init; }
    public int TeachersCount { get; init; }
    public int StudentsCount { get; init; }

    public void Deconstruct(out string id, out string title, out int teachersCount, out int studentsCount)
    {
        id = this.Id;
        title = this.Title;
        teachersCount = this.TeachersCount;
        studentsCount = this.StudentsCount;
    }
}
