using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = nameof(Health))]
        public IActionResult Health() => Ok(new { status = "ok" });
    }
}
