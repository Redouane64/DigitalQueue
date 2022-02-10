using DigitalQueue.Web.Data.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Dashboard.Pages;

[Authorize("Admin")]
public class IndexModel : PageModel
{
    public IEnumerable<User> Users { get; set; }

    public IEnumerable<Course> Courses { get; set; }

    public void OnGet()
    {

    }
}
