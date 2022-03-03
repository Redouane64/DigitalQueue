using DigitalQueue.Web.Areas.Courses.Dtos;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public class GetCourseByQuery : IRequest<CourseDto?>
{
    public string? Name { get; }

    public int? Year { get; set; }
    

    public GetCourseByQuery(string name)
    {
        this.Name = name;
    }

    public class GetCourseByNameHandler : IRequestHandler<GetCourseByQuery, CourseDto?>
    {
        private readonly DigitalQueueContext _context;

        public GetCourseByNameHandler(DigitalQueueContext context)
        {
            _context = context;
        }
        
        public async Task<CourseDto?> Handle(GetCourseByQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Courses
                .AsNoTracking();

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
            
            return new CourseDto(course.Id, course.Title);
        }
    }
}
