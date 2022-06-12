using DigitalQueue.Web.Areas.Courses.Dtos;

using MediatR;

namespace DigitalQueue.Web.Areas.Courses.Queries;

public partial class FindCourseByTitleQuery : IRequest<CourseDto?>
{
    public string? Name { get; }

    public int? Year { get; set; }


    public FindCourseByTitleQuery(string name)
    {
        this.Name = name;
    }
}
