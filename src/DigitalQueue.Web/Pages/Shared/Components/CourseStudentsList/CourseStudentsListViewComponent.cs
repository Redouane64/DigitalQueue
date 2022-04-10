using DigitalQueue.Web.Areas.Courses.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Pages.Shared.Components.CourseStudentsList;

[ViewComponent]
public class CourseStudentsListViewComponent : ViewComponent
{
    private readonly IMediator _mediator;

    public CourseStudentsListViewComponent(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IViewComponentResult> InvokeAsync(string courseId)
    {
        var teachers = await this._mediator.Send(new GetCourseStudentsQuery(courseId));
        return View(teachers);
    }
}
