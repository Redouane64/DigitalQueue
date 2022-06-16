using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Pages.Shared.Components.PermissionsList;

public class PermissionsListViewComponent : ViewComponent
{
    public PermissionsListViewComponent()
    {
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}
