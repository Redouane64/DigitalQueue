using DigitalQueue.Web.Areas.Courses.Models;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public class FindCourseByNameQuery : IRequest<CourseResult?>
{
    public string Name { get; }
    public string? Group { get; set; }
    public int? Year { get; set; }
    
    public FindCourseByNameQuery(string name)
    {
        this.Name = name;
    }
}

public class FindCourseByNameQueryHandler : IRequestHandler<FindCourseByNameQuery, CourseResult?>
{
    private readonly DigitalQueueContext _context;

    public FindCourseByNameQueryHandler(DigitalQueueContext context)
    {
        _context = context;
    }

    public async Task<CourseResult?> Handle(FindCourseByNameQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Courses
            .AsNoTracking()
            .Where(c => !c.Deleted && c.Name.ToLower() == request.Name.ToLower());

        if (request.Group is not null)
        {
            query = query.Where(c => c.Group.ToLower() == request.Name.ToLower());
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

        return new CourseResult
        {
            Id = course.Id,
            Title = course.Name,
            Year = course.Year,
            CreatedAt = course.CreateAt
        };
    }
}
