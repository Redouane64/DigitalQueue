using System.Security.Claims;

using DigitalQueue.Web.Areas.Courses.Dtos;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public class GetQueueByCourseIdQuery : IRequest<IEnumerable<QueueItemDto>>
{
    public string CourseId { get; }

    public GetQueueByCourseIdQuery(string courseId)
    {
        CourseId = courseId;
    }
    
    public class GetQueueByCourseIdQueryHandler : IRequestHandler<GetQueueByCourseIdQuery, IEnumerable<QueueItemDto>>
    {
        private readonly DigitalQueueContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetQueueByCourseIdQueryHandler(DigitalQueueContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<IEnumerable<QueueItemDto>> Handle(GetQueueByCourseIdQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            return await _context.Queues.AsNoTracking()
                .Include(e => e.Creator)
                .Where(e => e.CourseId == request.CourseId)
                .Where(e => e.Course.Teachers.Any(t => t.Id == currentUserId))
                .Where(e => e.Completed != true)
                .Select(e => new QueueItemDto() { Id = e.Id, Student = e.Creator.Name, CreatedAt = e.CreateAt, })
                .OrderBy(e => e.CreatedAt)
                .ToArrayAsync(cancellationToken);
        }
    }
}
