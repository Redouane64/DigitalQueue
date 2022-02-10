using DigitalQueue.Web.Data.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Courses.Pages;

[Authorize("Admin")]
public class Index : PageModel
{
    public IEnumerable<Course> Courses { get; set; }
    
    public void OnGet()
    {
        
    }
}
