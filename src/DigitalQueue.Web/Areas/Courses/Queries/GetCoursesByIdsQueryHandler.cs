using DigitalQueue.Web.Areas.Courses.Models;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public class GetCoursesByIdsQuery : IRequest<IEnumerable<CourseResult>>
{
    public string[] Ids { get; }

    public GetCoursesByIdsQuery(string[] ids)
    {
        Ids = ids;
    }
}

public class GetCoursesByIdsQueryHandler : IRequestHandler<GetCoursesByIdsQuery, IEnumerable<CourseResult>>
{
    private readonly DigitalQueueContext _context;

    public GetCoursesByIdsQueryHandler(DigitalQueueContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CourseResult>> Handle(GetCoursesByIdsQuery request, CancellationToken cancellationToken)
    {
        return await this._context.Courses
            .AsNoTracking()
            .Where(c => !c.Deleted)
            .Where(c => request.Ids.Contains(c.Id))
            .Select(course => new CourseResult
            {
                Id = course.Id,
                Title = course.Name,
                Year = course.Year,
                CreatedAt = course.CreateAt
            })
            .ToArrayAsync(cancellationToken);
    }
}

