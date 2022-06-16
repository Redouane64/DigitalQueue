using DigitalQueue.Web.Areas.Courses.Dtos;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Queries;


public class GetCourseQueryHandler : IRequestHandler<GetCoursesQuery, IEnumerable<CourseDto>>
{
    private readonly DigitalQueueContext _context;

    public GetCourseQueryHandler(DigitalQueueContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CourseDto>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Courses
            .AsNoTracking()
            .OrderByDescending(c => c.CreateAt)
            .Where(c => !c.IsArchived);

        if (request.SearchQuery is not null)
        {
            query = query.Where(c => EF.Functions.Like(c.Title, $"%{request.SearchQuery}%"));
        }

        return await query.Select(
            course => new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Year = course.Year,
                CreatedAt = course.CreateAt,
                Teachers = course.Teachers.Count,
                Students = course.QueueItems.Count,
            }
        ).ToArrayAsync(cancellationToken);
    }
}

