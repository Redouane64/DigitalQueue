using MediatR;

namespace DigitalQueue.Web.Areas.Courses.Commands;

public partial class CompleteQueueItemCommand : IRequest
{
    public string RequestId { get; }

    public CompleteQueueItemCommand(string requestId)
    {
        RequestId = requestId;
    }
}
