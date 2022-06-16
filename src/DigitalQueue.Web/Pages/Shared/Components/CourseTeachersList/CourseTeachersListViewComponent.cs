using DigitalQueue.Web.Areas.Users.Queries;
using DigitalQueue.Web.Areas.Users.Queries.Teachers;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Pages.Shared.Components.CourseTeachersList;

[ViewComponent]
public class CourseTeachersListViewComponent : ViewComponent
{
    private readonly IMediator _mediator;

    public CourseTeachersListViewComponent(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IViewComponentResult> InvokeAsync(string courseId)
    {
        var teachers = await this._mediator.Send(new GetTeachersByCourseIdQuery(courseId));
        return View(teachers);
    }
}
