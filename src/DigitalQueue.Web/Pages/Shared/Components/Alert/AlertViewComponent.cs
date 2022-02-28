using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Pages.Shared.Components.Alert;

public class AlertViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(bool success, string message)
    {
        return View(new { success, message });
    }
}
