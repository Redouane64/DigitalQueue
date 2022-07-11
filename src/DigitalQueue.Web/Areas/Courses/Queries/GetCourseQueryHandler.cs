using DigitalQueue.Web.Areas.Courses.Models;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public class GetCoursesQuery : IRequest<IEnumerable<CourseResult>>
{
    public string? SearchQuery { get; }

    public GetCoursesQuery(string? searchQuery)
    {
        SearchQuery = searchQuery;
    }
}

public class GetCourseQueryHandler : IRequestHandler<GetCoursesQuery, IEnumerable<CourseResult>>
{
    private readonly DigitalQueueContext _context;

    public GetCourseQueryHandler(DigitalQueueContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CourseResult>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Courses
            .AsNoTracking()
            .OrderByDescending(c => c.CreateAt)
            .Where(c => !c.Deleted);

        if (request.SearchQuery is not null)
        {
            query = query.Where(c => EF.Functions.Like(c.Name, $"%{request.SearchQuery}%"));
        }

        return await query.Select(
            course => new CourseResult
            {
                Id = course.Id,
                Title = course.Name,
                Year = course.Year,
                CreatedAt = course.CreateAt,
                Teachers = course.Teachers.Count,
                Students = course.QueueItems.Count,
            }
        ).ToArrayAsync(cancellationToken);
    }
}

