using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Courses.Pages;

[Authorize("Admin")]
public class CreateCourseModel : PageModel
{
    public string Title { get; set; }
    
    public void OnGet()
    {
        
    }

    public void OnPost()
    {
        
    }
}
