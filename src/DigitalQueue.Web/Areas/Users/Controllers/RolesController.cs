using DigitalQueue.Web.Areas.Accounts.Models;
using DigitalQueue.Web.Areas.Common.Models;
using DigitalQueue.Web.Areas.Users.Commands;
using DigitalQueue.Web.Areas.Users.Commands.Roles;
using DigitalQueue.Web.Areas.Users.Models;

using MediatR;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Areas.Users.Controllers
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

        [Authorize(
            AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, 
            Roles = "Admin"
        )]
        [HttpPost("search", Name = nameof(SearchRoles))]
        [Consumes("application/x-www-form-urlencoded")]
        [Produces(typeof(SearchResult<AccountRole>))]
        public async Task<IActionResult> SearchRoles([FromForm] string q)
        {
            return Ok(await _mediator.Send(new SearchRoleCommand(query: q)));
        }
    }
}
