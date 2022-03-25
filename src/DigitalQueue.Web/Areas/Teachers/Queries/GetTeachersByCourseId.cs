using DigitalQueue.Web.Areas.Teachers.Dtos;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace DigitalQueue.Web.Areas.Teachers.Queries;

public class GetTeachersByCourseId : IRequest<IEnumerable<TeacherDto>>
{
    public string CourseId { get; }

    public GetTeachersByCourseId(string courseId)
    {
        CourseId = courseId;
    }
    
    public class GetTeachersByCourseIdQueryHandler : IRequestHandler<GetTeachersByCourseId, IEnumerable<TeacherDto>>
    {
        private readonly DigitalQueueContext _context;

        public GetTeachersByCourseIdQueryHandler(DigitalQueueContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<TeacherDto>> Handle(GetTeachersByCourseId request, CancellationToken cancellationToken)
        {

            var course = await this._context.Courses
                .Include(c => c.Teachers)
                .FirstOrDefaultAsync(c => c.Id == request.CourseId);

            if (course is null)
            {
                return Array.Empty<TeacherDto>();
            }

            var teachers = course.Teachers.Select(t => new TeacherDto(t.Name, t.Id));

            return teachers;
        }
    }
}
