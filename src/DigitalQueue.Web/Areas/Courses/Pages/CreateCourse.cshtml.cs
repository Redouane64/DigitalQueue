using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using DigitalQueue.Web.Areas.Courses.Commands;
using DigitalQueue.Web.Areas.Courses.Queries;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Courses.Pages;

[Authorize("Admin")]
public class CreateCourseModel : PageModel
{
    private readonly IMediator _mediator;

    public CreateCourseModel(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [BindProperty]
    [Required]
    public string Title { get; set; }

    [BindProperty]
    [Required]
    public string[] Teachers { get; set; }

    // TODO: add validation
    public int? Year { get; set; }

    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPost()
    {
        var existingCourse = await this._mediator.Send(new FindCourseByTitleQuery(Title));

        if (existingCourse is not null)
        {
            ModelState.AddModelError(nameof(Title), "Course with same name and year already exists.");
            return Page();
        }
        
        var course = await this._mediator.Send(new CreateCourseCommand(Title, Teachers));

        if (course is null)
        {
            ModelState.AddModelError("", "Unable to create course");
            return Page();
        }

        HttpContext.Session.SetInt32("redirect_from_success_create", 1);
        return RedirectToPagePermanent("Index");
    }
}
