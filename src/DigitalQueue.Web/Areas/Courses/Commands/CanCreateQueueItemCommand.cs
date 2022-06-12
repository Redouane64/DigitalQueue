using MediatR;

namespace DigitalQueue.Web.Areas.Courses.Commands;

public partial class CanCreateQueueItemCommand : IRequest<bool>
{
    public string CourseId { get; }

    public CanCreateQueueItemCommand(string courseId)
    {
        CourseId = courseId;
    }
}
