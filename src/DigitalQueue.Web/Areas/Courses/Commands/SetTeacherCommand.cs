using MediatR;

namespace DigitalQueue.Web.Areas.Courses.Commands;

public partial class SetTeacherCommand : IRequest
{
    public string CourseId { get; }
    public string[] Teachers { get; }

    public SetTeacherCommand(string courseId, string[] teachers)
    {
        CourseId = courseId;
        Teachers = teachers;
    }
}
