using DigitalQueue.Web.Data.Entities;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Courses.Pages;

public class Index : PageModel
{
    public IEnumerable<Course> Courses { get; set; }
    
    public void OnGet()
    {
        
    }
}
