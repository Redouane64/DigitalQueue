using System.Security.Claims;

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
        private readonly ILogger<GetCourseRequestsQueryHandler> _logger;

        public GetCourseRequestsQueryHandler(
            DigitalQueueContext context,
            ILogger<GetCourseRequestsQueryHandler> logger)
        {
            _context = context;
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

                var received = await _context.Queues
                    .Include(q => q.Course)
                    .ThenInclude(
                        q => q.Teachers
                            .Where(t => t.Id == requestQuery.UserId)
                    )
                    .Include(q => q.Creator)
                    .Where(q => q.Creator.Id != requestQuery.UserId)
                    .GroupBy(q => q.Course.Title)
                    .Select(q => new CourseQueueDto(q.Key, q.Count(), q.Select(e => new QueueItemDto
                    {
                        Id = e.Id,
                        CreatedAt = e.CreateAt,
                        Student = e.Creator.Name,
                    })))
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
