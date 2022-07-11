using System.Security.Claims;

using DigitalQueue.Web.Areas.Courses.Models;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public class GetQueueByCourseIdQuery : IRequest<IEnumerable<QueueItemDto>>
{
    public string CourseId { get; }
    public bool Received { get; set; }
    public ClaimsPrincipal User { get; set; }

    public GetQueueByCourseIdQuery(string courseId)
    {
        CourseId = courseId;
    }
}

public class GetQueueByCourseIdQueryHandler : IRequestHandler<GetQueueByCourseIdQuery, IEnumerable<QueueItemDto>>
{
    private readonly DigitalQueueContext _context;

    public GetQueueByCourseIdQueryHandler(DigitalQueueContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<QueueItemDto>> Handle(GetQueueByCourseIdQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = request.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var query = _context.Queues.AsNoTracking()
            .Include(e => e.Creator)
            .Where(e => e.CourseId == request.CourseId)
            .Where(e => e.Completed != true);

        if (request.Received)
        {
            query = query.Where(e => e.CreatorId != currentUserId);
        }

        return await query.OrderBy(e => e.CreateAt)
                          .Select(e => new QueueItemDto {
                                Id = e.Id,
                                Student = e.Creator.Name,
                                CreatedAt = e.CreateAt,
                                You = currentUserId == e.CreatorId,
                          })
                          .ToArrayAsync(cancellationToken);
    }
}

