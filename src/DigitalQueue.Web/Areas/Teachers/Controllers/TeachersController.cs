using DigitalQueue.Web.Areas.Common.Dtos;
using DigitalQueue.Web.Areas.Teachers.Dtos;
using DigitalQueue.Web.Areas.Teachers.Queries;

using MediatR;

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
    private readonly IMediator _mediator;

    public TeachersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [HttpPost("search", Name = nameof(Search))]
    [Consumes("application/x-www-form-urlencoded")]
    [Produces(typeof(SearchResult<TeacherDto>))]
    public async Task<IActionResult> Search([FromForm] string q)
    {
        return Ok(await _mediator.Send(new SearchTeacherQuery(q)));
    }
}
