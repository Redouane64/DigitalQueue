using DigitalQueue.Web.Areas.Teachers.Services;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Areas.Teachers.Controllers;

[Route("/api/[controller]")]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
public class TeachersController : ControllerBase
{
    private readonly TeachersService _teachersService;

    public TeachersController(TeachersService teachersService)
    {
        _teachersService = teachersService;
    }
    
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [HttpPost("search", Name = nameof(Search))]
    public async Task<IActionResult> Search([FromQuery(Name = "q")] string? q)
    {
        // TODO: update logic to work with database
        return Ok(await this._teachersService.Search(q));
    }
}
