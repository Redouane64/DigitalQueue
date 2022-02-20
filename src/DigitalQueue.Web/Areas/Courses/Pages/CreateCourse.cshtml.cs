using System.ComponentModel.DataAnnotations;

using DigitalQueue.Web.Areas.Courses.Commands;

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

    public void OnGet()
    {
        
    }

    public async Task OnPost()
    {
        await this._mediator.Send(new CreateCourseCommand(Title, Teachers));
    }
}
