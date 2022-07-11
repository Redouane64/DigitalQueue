using DigitalQueue.Web.Areas.Courses.Models;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Users;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public partial class GetCourseStudentsQuery : IRequest<IEnumerable<CourseStudentDto>>
{
    public string CourseId { get; }

    public GetCourseStudentsQuery(string courseId)
    {
        CourseId = courseId;
    }
}

public class GetCourseStudentsQueryHandler : IRequestHandler<GetCourseStudentsQuery, IEnumerable<CourseStudentDto>>
{
    private readonly DigitalQueueContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<GetCourseStudentsQueryHandler> _logger;

    public GetCourseStudentsQueryHandler(DigitalQueueContext context, UserManager<ApplicationUser> userManager, ILogger<GetCourseStudentsQueryHandler> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<IEnumerable<CourseStudentDto>> Handle(GetCourseStudentsQuery request, CancellationToken cancellationToken)
    {
        var students = await _context.Queues.AsNoTracking()
            .Where(r => r.CourseId == request.CourseId)
            .Select(r => new { r.CreatorId })
            .Distinct()
            .Join(_context.Users,
                e => e.CreatorId,
                u => u.Id,
                (r, u) => new CourseStudentDto(u.Name, u.Id)
            )
            .ToArrayAsync(cancellationToken);

        return students;
    }
}

