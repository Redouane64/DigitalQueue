using DigitalQueue.Web.Areas.Courses.Dtos;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public class GetCourseByName : IRequest<CourseDto?>
{
    public string Name { get; }

    public GetCourseByName(string name)
    {
        this.Name = name;
    }

    public class GetCourseByNameHandler : IRequestHandler<GetCourseByName, CourseDto?>
    {
        private readonly DigitalQueueContext _context;

        public GetCourseByNameHandler(DigitalQueueContext context)
        {
            _context = context;
        }
        
        public async Task<CourseDto?> Handle(GetCourseByName request, CancellationToken cancellationToken)
        {
            var course = await _context.Courses
                .AsNoTracking()
                .Where(c => c.Title.ToLower() == request.Name.ToLower())
                .FirstOrDefaultAsync(cancellationToken);

            if (course is null)
            {
                return null;
            }

            // TODO: make count args optional
            return new CourseDto(course.Id, course.Title, 0, 0);
        }
    }
}
