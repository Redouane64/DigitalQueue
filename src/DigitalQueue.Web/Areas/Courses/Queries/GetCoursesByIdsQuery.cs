using DigitalQueue.Web.Areas.Courses.Dtos;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public class GetCoursesByIdsQuery : IRequest<IEnumerable<CourseDto>>
{
    public string[] Ids { get; }

    public GetCoursesByIdsQuery(string[] ids)
    {
        Ids = ids;
    }

    public class GetCoursesByIdsQueryHandler : IRequestHandler<GetCoursesByIdsQuery, IEnumerable<CourseDto>>
    {
        private readonly DigitalQueueContext _context;

        public GetCoursesByIdsQueryHandler(DigitalQueueContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourseDto>> Handle(GetCoursesByIdsQuery request, CancellationToken cancellationToken)
        {
            return await this._context.Courses
                .AsNoTracking()
                .Where(c => !c.IsArchived)
                .Where(c => request.Ids.Contains(c.Id))
                .Select(course => new CourseDto
                {
                    Id = course.Id,
                    Title = course.Title,
                    Year = course.Year,
                    CreatedAt = course.CreateAt
                })
                .ToArrayAsync(cancellationToken);
        }
    }
}
