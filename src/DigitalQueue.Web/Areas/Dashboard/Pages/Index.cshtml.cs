using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Dashboard.Pages;

[Authorize("Admin")]
public class IndexModel : PageModel
{
    public void OnGet()
    {

    }
}
