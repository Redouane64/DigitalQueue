using DigitalQueue.Web.Areas.Courses.Dtos;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public class GetCourseRequestsQuery : IRequest<RequestsQueueDto>
{
    public string UserId { get; }

    public GetCourseRequestsQuery(string userId)
    {
        UserId = userId;
    }
    
    public class GetCourseRequestsQueryHandler : IRequestHandler<GetCourseRequestsQuery, RequestsQueueDto>
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
        
        public async Task<RequestsQueueDto> Handle(GetCourseRequestsQuery requestQuery, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            var sent = await _context.Requests.Include(e => e.Course)
                .Include(e => e.Creator)
                .Where(r => r.CreatorId == requestQuery.UserId)
                .GroupBy(r => r.Course.Title)
                .Select(r => new
                UserRequestsDto(r.Key, r.Count(), r.Select(e => new RequestInfoDto
                {
                    RequestId = e.Id,
                    CreatedAt = e.CreateAt
                })))
                .ToArrayAsync(cancellationToken);

            var courseRequests = await _context.CourseRequests.FromSqlInterpolated(@$"
                    select
                           r.id as RequestId,
                           r.create_at as CreatedAt,
                           c.id as CourseId, 
                           c.title as CourseTitle, 
                           c.year as CourseYear, 
                           r.creator_id as StudentId, 
                           a.name as StudentName
                    from users as u
                    join course_teacher ct on u.id = ct.teacher_id
                    join requests r on ct.course_id = r.course_id
                    join courses c on ct.course_id = c.id
                    join users a on r.creator_id = a.id
                    where u.id = {requestQuery.UserId}
                ")
                .ToArrayAsync(cancellationToken);
                
                var received = courseRequests
                .GroupBy(r => r.CourseTitle)
                .Select(r => 
                    new CourseRequestsDto(
                        r.Key, 
                        r.Count(), 
                        r.Select(e => new RequestInfoDto
                        {
                            RequestId = e.RequestId,
                            CreatedAt = e.CreatedAt,
                            StudentName = e.StudentName,
                        })
                    )
                )
                .ToArray();

            await transaction.CommitAsync(cancellationToken);

            return new RequestsQueueDto(sent, received);
        }
    }
}
