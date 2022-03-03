using DigitalQueue.Web.Areas.Courses.Dtos;
using DigitalQueue.Web.Areas.Courses.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Pages.Shared.Components.CoursesList;

public class CoursesListViewComponent : ViewComponent
{
    private readonly IMediator _mediator;

    public CoursesListViewComponent(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IViewComponentResult> InvokeAsync(int? page, int? pageSize, bool fullList = true)
    {
        var courses = await _mediator.Send(new GetCoursesQuery());
        return View(new CoursesListDto(courses, fullView: fullList));
    }
}
