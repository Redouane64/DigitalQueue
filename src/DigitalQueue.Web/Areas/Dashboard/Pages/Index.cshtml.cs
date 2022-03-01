using DigitalQueue.Web.Areas.Courses.Dtos;
using DigitalQueue.Web.Areas.Courses.Queries;

using MediatR;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Dashboard.Pages;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "Admin")]
public class Index : PageModel
{
    private readonly IMediator _mediator;

    public Index(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public IEnumerable<CourseDto> Courses { get; set; }

    public async Task OnGet()
    {
        Courses = await this._mediator.Send(new GetCoursesQuery());
    }
}
