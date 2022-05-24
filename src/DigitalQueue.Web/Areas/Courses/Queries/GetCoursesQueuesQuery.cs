
using DigitalQueue.Web.Areas.Courses.Dtos;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public class GetCoursesQueuesQuery : IRequest<QueuesDto>
{
    public string UserId { get; }

    public GetCoursesQueuesQuery(string userId)
    {
        UserId = userId;
    }

    public class GetCoursesQueuesQueryHandler : IRequestHandler<GetCoursesQueuesQuery, QueuesDto>
    {
        private readonly DigitalQueueContext _context;
        private readonly ILogger<GetCoursesQueuesQueryHandler> _logger;

        public GetCoursesQueuesQueryHandler(
            DigitalQueueContext context,
            ILogger<GetCoursesQueuesQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<QueuesDto> Handle(GetCoursesQueuesQuery requestQuery, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var sent = await _context.Queues
                    .Where(r => r.CreatorId == requestQuery.UserId)
                    .Where(r => r.Completed == false)
                    .GroupBy(r => new { r.Course.Title, r.CourseId })
                    .Select(r => new CourseQueueDto(r.Key.Title, r.Key.CourseId, r.Count()))
                    .ToArrayAsync(cancellationToken);

                var received = await _context.Queues
                    .OrderByDescending(e => e.CreateAt)
                    .Where(q => q.Course.IsArchived == false)
                    .Where(c => c.Course.Teachers.Any(t => t.Id == requestQuery.UserId))
                    .Where(q => q.Creator.Id != requestQuery.UserId)
                    .Where(q => q.Completed == false)
                    .GroupBy(q => new { q.Course.Title, q.CourseId })
                    .Select(r => new CourseQueueDto(r.Key.Title, r.Key.CourseId, r.Count()))
                    .ToArrayAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return new QueuesDto(sent, received);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to fetch course requests");
            }

            return new QueuesDto(Enumerable.Empty<CourseQueueDto>(), Enumerable.Empty<CourseQueueDto>());
        }
    }
}
