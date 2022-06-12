using DigitalQueue.Web.Areas.Teachers.Dtos;

using MediatR;

namespace DigitalQueue.Web.Areas.Teachers.Queries;

public partial class GetTeachersByCourseIdQuery : IRequest<IEnumerable<TeacherDto>>
{
    public string CourseId { get; }

    public GetTeachersByCourseIdQuery(string courseId)
    {
        CourseId = courseId;
    }
}
