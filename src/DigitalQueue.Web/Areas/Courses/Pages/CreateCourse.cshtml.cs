using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Courses.Pages;

[Authorize("Admin")]
public class CreateCourseModel : PageModel
{
    [BindProperty]
    [Required]
    public string Title { get; set; }

    [BindProperty]
    [Required]
    public string[] Teachers { get; set; }

    public void OnGet()
    {
        
    }

    public void OnPost()
    {
        
    }
}
