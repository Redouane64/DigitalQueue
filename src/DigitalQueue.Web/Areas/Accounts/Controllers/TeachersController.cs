using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DigitalQueue.Web.Users.Dtos;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Areas.Accounts.Controllers;

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
            new { text = "Jack", id=1 }, 
            new { text = "Karim", id=2 }, 
            new { text = "Joe", id=3 }, 
        }, new {more = false});
        return Ok(sample);
    }
}
