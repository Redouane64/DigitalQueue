using DigitalQueue.Web.Areas.Courses.Dtos;

using MediatR;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public partial class GetQueueByCourseIdQuery : IRequest<IEnumerable<QueueItemDto>>
{
    public string CourseId { get; }

    public bool Received { get; }

    public GetQueueByCourseIdQuery(string courseId, bool received)
    {
        CourseId = courseId;
        Received = received;
    }
}
