using DigitalQueue.Web.Areas.Teachers.Dtos;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Teachers.Queries;

public partial class GetTeachersByCourseIdQuery
{
    public class GetTeachersByCourseIdQueryHandler : IRequestHandler<GetTeachersByCourseIdQuery, IEnumerable<TeacherDto>>
    {
        private readonly DigitalQueueContext _context;

        public GetTeachersByCourseIdQueryHandler(DigitalQueueContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeacherDto>> Handle(GetTeachersByCourseIdQuery request, CancellationToken cancellationToken)
        {

            var course = await this._context.Courses
                .AsNoTracking()
                .Include(c => c.Teachers)
                .FirstOrDefaultAsync(c => c.Id == request.CourseId, cancellationToken: cancellationToken);

            if (course is null)
            {
                return Array.Empty<TeacherDto>();
            }

            var teachers = course.Teachers.Select(t => new TeacherDto(t.Name, t.Id));

            return teachers;
        }
    }
}
