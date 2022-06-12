using DigitalQueue.Web.Areas.Courses.Dtos;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public partial class FindCourseByTitleQuery
{
    public class GetCourseByNameHandler : IRequestHandler<FindCourseByTitleQuery, CourseDto?>
    {
        private readonly DigitalQueueContext _context;

        public GetCourseByNameHandler(DigitalQueueContext context)
        {
            _context = context;
        }

        public async Task<CourseDto?> Handle(FindCourseByTitleQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Courses
                .AsNoTracking()
                .Where(c => !c.IsArchived);

            if (request.Name is not null)
            {
                query = query.Where(c => c.Title.ToLower() == request.Name.ToLower());
            }

            if (request.Year is not null)
            {
                query = query.Where(c => c.Year == request.Year);
            }

            var course = await query.FirstOrDefaultAsync(cancellationToken);

            if (course is null)
            {
                return null;
            }

            return new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Year = course.Year,
                CreatedAt = course.CreateAt
            };
        }
    }
}
