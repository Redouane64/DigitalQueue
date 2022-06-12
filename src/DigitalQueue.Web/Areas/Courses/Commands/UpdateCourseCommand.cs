using MediatR;

namespace DigitalQueue.Web.Areas.Courses.Commands;

public partial class UpdateCourseCommand : IRequest<bool>
{
    public string Id { get; }
    public string? Title { get; }
    public int Year { get; }

    public UpdateCourseCommand(string id, string? title, int year)
    {
        Id = id;
        Title = title;
        Year = year;
    }
}
