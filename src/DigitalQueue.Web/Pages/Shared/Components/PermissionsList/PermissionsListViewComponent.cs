using DigitalQueue.Web.Areas.Accounts.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Pages.Shared.Components.PermissionsList;

public class PermissionsListViewComponent : ViewComponent
{

    public IViewComponentResult Invoke(IEnumerable<ClaimDto> claims)
    {
        return View(new UserPermissionsDto(claims));
    }
}
