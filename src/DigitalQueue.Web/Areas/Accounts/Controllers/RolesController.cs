using DigitalQueue.Web.Areas.Accounts.Commands;

using MediatR;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Areas.Accounts.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpPost("search", Name = nameof(SearchRoles))]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> SearchRoles([FromForm] string q)
        {
            return Ok(await _mediator.Send(new SearchRoleCommand(q)));
        }
    }
}
