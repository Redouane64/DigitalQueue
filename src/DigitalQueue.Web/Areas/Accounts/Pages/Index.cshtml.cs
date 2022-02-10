using DigitalQueue.Web.Data.Entities;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Accounts.Pages;

public class Index : PageModel
{

    public IEnumerable<User> Users { get; set; }
    
    public void OnGet()
    {
        
    }
}
