using MediatR;

namespace DigitalQueue.Web.Areas.Courses.Commands;

public partial class ArchiveCourseCommand : IRequest
{
    public string CourseId { get; }

    public ArchiveCourseCommand(string courseId)
    {
        CourseId = courseId;
    }
}
