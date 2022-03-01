using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Pages.Shared.Components.RolesList;

public class RolesListViewComponent : ViewComponent
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RolesListViewComponent(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public IViewComponentResult Invoke(IEnumerable<RoleDto> roles, string userId)
    {
        var editable = _httpContextAccessor.HttpContext
            .User.FindFirstValue(ClaimTypes.NameIdentifier).Equals(userId);
        
        return View(new UserRolesDto(roles, userId, editable));
    }
}
