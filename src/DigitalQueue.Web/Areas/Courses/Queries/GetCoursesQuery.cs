using DigitalQueue.Web.Areas.Courses.Dtos;

using MediatR;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public partial class GetCoursesQuery : IRequest<IEnumerable<CourseDto>>
{
    public string? SearchQuery { get; }

    public GetCoursesQuery(string? searchQuery)
    {
        SearchQuery = searchQuery;
    }
}
