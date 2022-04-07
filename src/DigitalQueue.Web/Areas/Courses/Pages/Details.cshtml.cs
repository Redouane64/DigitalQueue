using DigitalQueue.Web.Areas.Courses.Commands;
using DigitalQueue.Web.Areas.Courses.Dtos;
using DigitalQueue.Web.Areas.Courses.Queries;

using MediatR;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Courses.Pages;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "Admin")]
public class Details : PageModel
{
    private readonly IMediator _mediator;

    public Details(IMediator mediator)
    {
        _mediator = mediator;
    }

    public CourseDto Course { get; set; }

    [TempData]
    public bool? PostResultMessage { get; set; }

    public async Task<IActionResult> OnGet([FromRoute]string courseId)
    {
        var course = await this._mediator.Send(new GetCoursesByIdsQuery(new[] {courseId}));
        if (!course.Any())
        {
            return NotFound();
        }

        Course = course.Single();

        return Page();
    }

    public async Task<IActionResult> OnPost([FromRoute]string courseId, [FromForm]string title, [FromForm]int year)
    {
        PostResultMessage = await this._mediator.Send(new UpdateCourseCommand(courseId, title, year));

        return RedirectToPagePermanent("Details", new {courseId});
    }

    public async Task<IActionResult> OnPostArchiveCourse([FromRoute] string courseId)
    {
        await _mediator.Send(new ArchiveCourseCommand(courseId));
        
        return RedirectToPagePermanent("Index");
    }
}
