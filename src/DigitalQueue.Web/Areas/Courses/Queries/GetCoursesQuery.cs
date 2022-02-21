using DigitalQueue.Web.Areas.Courses.Dtos;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public class GetCoursesQuery : IRequest<IEnumerable<CourseDto>>
{
    
    public class GetCourseQueryHandler : IRequestHandler<GetCoursesQuery, IEnumerable<CourseDto>>
    {
        private readonly DigitalQueueContext _context;

        public GetCourseQueryHandler(DigitalQueueContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<CourseDto>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
        {
            var courses = await _context.Courses
                .Include(c => c.Teachers)
                .Select(
                    c => new CourseDto(
                        c.Id,
                        c.Title, 
                        c.Teachers.Count, 
                        0 /* TODO: */
                    )
                )
                .ToArrayAsync(cancellationToken);

            return courses;
        }
    }
}
