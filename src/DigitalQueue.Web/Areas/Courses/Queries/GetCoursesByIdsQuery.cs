using DigitalQueue.Web.Areas.Courses.Dtos;

using MediatR;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public partial class GetCoursesByIdsQuery : IRequest<IEnumerable<CourseDto>>
{
    public string[] Ids { get; }

    public GetCoursesByIdsQuery(string[] ids)
    {
        Ids = ids;
    }
}
