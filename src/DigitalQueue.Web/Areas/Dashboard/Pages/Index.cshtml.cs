using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Accounts.Queries;
using DigitalQueue.Web.Areas.Courses.Dtos;
using DigitalQueue.Web.Areas.Courses.Queries;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Dashboard.Pages;

[Authorize("Admin")]
public class Index : PageModel
{
    private readonly IMediator _mediator;

    public Index(IMediator mediator)
    {
        _mediator = mediator;
    }

    public IEnumerable<UserDto> Users { get; set; }

    public IEnumerable<CourseDto> Courses { get; set; }

    public async Task OnGet()
    {
        ViewData["fullList"] = false;
        Users = await this._mediator.Send(new GetUsersQuery());
        Courses = await this._mediator.Send(new GetCoursesQuery());
    }
}
