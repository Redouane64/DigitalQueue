using DigitalQueue.Web.Areas.Users.Models;

using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Pages.Shared.Components.RolesList;

public class RolesListViewComponent : ViewComponent
{
    public RolesListViewComponent()
    {
    }

    public IViewComponentResult Invoke(IEnumerable<AccountRole> roles)
    {
        return View(roles);
    }
}
