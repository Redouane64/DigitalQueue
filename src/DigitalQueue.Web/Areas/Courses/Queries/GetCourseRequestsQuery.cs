using DigitalQueue.Web.Areas.Courses.Dtos;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public class GetCourseRequestsQuery : IRequest<QueueDto>
{
    public string UserId { get; }

    public GetCourseRequestsQuery(string userId)
    {
        UserId = userId;
    }
    
    public class GetCourseRequestsQueryHandler : IRequestHandler<GetCourseRequestsQuery, QueueDto>
    {
        private readonly DigitalQueueContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<GetCourseRequestsQueryHandler> _logger;

        public GetCourseRequestsQueryHandler(
            DigitalQueueContext context,
            UserManager<User> userManager,
            ILogger<GetCourseRequestsQueryHandler> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        
        public async Task<QueueDto> Handle(GetCourseRequestsQuery requestQuery, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var sent = await _context.Queues.Include(e => e.Course)
                    .Include(e => e.Creator)
                    .Where(r => r.CreatorId == requestQuery.UserId)
                    .GroupBy(r => r.Course.Title)
                    .Select(r => new
                        UserQueueDto(r.Key, r.Count(), r.Select(e => new QueueItemDto
                        {
                            Id = e.Id,
                            CreatedAt = e.CreateAt
                        })))
                    .ToArrayAsync(cancellationToken);

                var items = await _context.Set<CourseQueueItem>()
                    .FromSqlInterpolated(@$"
                        select
                               q.id as ItemId,
                               q.create_at as CreatedAt, 
                               c.title as Course,
                               a.name as Student
                        from users as u
                        join course_teacher ct on u.id = ct.teacher_id
                        join queue_items q on ct.course_id = q.course_id
                        join courses c on ct.course_id = c.id
                        join users a on q.creator_id = a.id
                        where u.id = {requestQuery.UserId}
                    ")
                    .ToArrayAsync(cancellationToken);
                
                var received = items
                    .GroupBy(r => r.Course)
                    .Select(r => 
                        new CourseQueueDto(
                            r.Key, 
                            r.Count(), 
                            r.Select(e => new QueueItemDto
                            {
                                Id = e.ItemId,
                                CreatedAt = e.CreatedAt,
                                Student = e.Student,
                            })
                            .OrderBy(e => e.CreatedAt)
                        )
                    )
                    .ToArray();

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
