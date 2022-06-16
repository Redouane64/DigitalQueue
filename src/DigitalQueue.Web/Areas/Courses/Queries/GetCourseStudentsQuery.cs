using DigitalQueue.Web.Areas.Courses.Dtos;

using MediatR;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public partial class GetCourseStudentsQuery : IRequest<IEnumerable<CourseStudentDto>>
{
    public string CourseId { get; }

    public GetCourseStudentsQuery(string courseId)
    {
        CourseId = courseId;
    }
}
