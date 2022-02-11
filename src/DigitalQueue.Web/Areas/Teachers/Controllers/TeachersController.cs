using DigitalQueue.Web.Users.Dtos;

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
    public TeachersController()
    {
        
    }
    
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [HttpPost("search", Name = nameof(Search))]
    public async Task<IActionResult> Search([FromQuery(Name = "q")] string? q)
    {
        // TODO: update logic to work with database
        
        var sample = new TeacherSearchResult(new[]
        {
            new { text = "Jack", id="33D2C7A7-CF3E-4DFB-9922-D7ABC4052BE1" }, 
            new { text = "Karim", id="63E0C9D4-D33C-4A09-BD56-0DB17F4236E6" }, 
            new { text = "Joe", id="BB56761A-B815-4450-81A6-356511C31DF8" }, 
        }, new {more = false});
        return Ok(sample);
    }
}
