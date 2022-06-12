using MediatR;
using MediatR.Pipeline;

namespace DigitalQueue.Web.Areas.Courses.Commands;

public partial class CreateQueueItemCommand : IRequest
{
    public string CourseId { get; }
    public string UserId { get; }

    public CreateQueueItemCommand(string courseId, string userId)
    {
        CourseId = courseId;
        UserId = userId;
    }
}
