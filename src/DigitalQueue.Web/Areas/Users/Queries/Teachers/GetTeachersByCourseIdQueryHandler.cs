using DigitalQueue.Web.Areas.Users.Models;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Users.Queries.Teachers;

public class GetTeachersByCourseIdQuery : IRequest<IEnumerable<TeacherAccount>>
{
    public string CourseId { get; }

    public GetTeachersByCourseIdQuery(string courseId)
    {
        CourseId = courseId;
    }
}

public class GetTeachersByCourseIdQueryHandler : IRequestHandler<GetTeachersByCourseIdQuery, IEnumerable<TeacherAccount>>
{
    private readonly DigitalQueueContext _context;

    public GetTeachersByCourseIdQueryHandler(DigitalQueueContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TeacherAccount>> Handle(GetTeachersByCourseIdQuery request, CancellationToken cancellationToken)
    {

        var course = await this._context.Courses
            .AsNoTracking()
            .Include(c => c.Teachers)
            .FirstOrDefaultAsync(c => c.Id == request.CourseId, cancellationToken: cancellationToken);

        if (course is null)
        {
            return Array.Empty<TeacherAccount>();
        }

        var teachers = course.Teachers.Select(t => new TeacherAccount(t.Name, t.Id));

        return teachers;
    }
}

