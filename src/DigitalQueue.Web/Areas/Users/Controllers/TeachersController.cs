using DigitalQueue.Web.Areas.Common.Models;
using DigitalQueue.Web.Areas.Users.Models;
using DigitalQueue.Web.Areas.Users.Queries;
using DigitalQueue.Web.Areas.Users.Queries.Teachers;

using MediatR;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Areas.Users.Controllers;

[Route("/api/[controller]")]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
public class TeachersController : ControllerBase
{
    private readonly IMediator _mediator;

    public TeachersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [HttpPost("search", Name = nameof(Search))]
    [Consumes("application/x-www-form-urlencoded")]
    [Produces(typeof(SearchResult<TeacherAccount>))]
    public async Task<IActionResult> Search([FromForm] string q)
    {
        return Ok(await _mediator.Send(new SearchTeacherQuery(q)));
    }
}
