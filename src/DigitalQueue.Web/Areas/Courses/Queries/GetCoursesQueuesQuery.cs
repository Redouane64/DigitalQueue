
using DigitalQueue.Web.Areas.Courses.Dtos;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public class GetCoursesQueuesQuery : IRequest<QueueDto>
{
    public string UserId { get; }

    public GetCoursesQueuesQuery(string userId)
    {
        UserId = userId;
    }
    
    public class GetCoursesQueuesQueryHandler : IRequestHandler<GetCoursesQueuesQuery, QueueDto>
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
        
        public async Task<QueueDto> Handle(GetCoursesQueuesQuery requestQuery, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var sent = await _context.Queues
                    .AsNoTracking()
                    .Include(e => e.Course)
                    .Include(e => e.Creator)
                    .Where(r => r.CreatorId == requestQuery.UserId && r.Completed == false)
                    .GroupBy(r => r.Course.Title)
                    .Select(r => new
                        UserQueueDto(
                            r.Key, 
                            r.Count(), 
                            r.OrderBy(e => e.CreateAt)
                             .Select(e => new QueueItemDto
                                {
                                    Id = e.Id,
                                    CreatedAt = e.CreateAt
                                }
                             )
                        )
                    )
                    .ToArrayAsync(cancellationToken);

                var received = await _context.Queues
                    .AsNoTracking()
                    .OrderByDescending(e => e.CreateAt)
                    .Include(q => q.Course)
                    .ThenInclude(q => q.Teachers)
                    .Include(q => q.Creator)
                    .Where(c => c.Course.Teachers.Any(t => t.Id == requestQuery.UserId))
                    .Where(q => q.Creator.Id != requestQuery.UserId)
                    .Where(q => q.Completed == false)
                    .GroupBy(q => q.Course.Title)
                    .Select(q => new CourseQueueDto(
                        q.Key, 
                        q.Count(), 
                        q.OrderBy(e => e.CreateAt)
                         .Select(e => new QueueItemDto
                                {
                                    Id = e.Id,
                                    CreatedAt = e.CreateAt,
                                    Student = e.Creator.Name,
                                }
                            )
                        )
                    )
                    .ToArrayAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return new QueueDto(sent, received);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to fetch course requests");
            }

            return new QueueDto(Array.Empty<UserQueueDto>(), Array.Empty<CourseQueueDto>());
        }
    }
}
