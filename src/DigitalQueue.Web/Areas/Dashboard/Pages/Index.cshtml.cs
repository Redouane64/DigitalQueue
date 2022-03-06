using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Dashboard.Pages;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "Admin")]
public class Index : PageModel
{

    public Index()
    {
    }
    
    public async Task OnGet()
    {
    }
}
