using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Sessions.Commands;
using DigitalQueue.Web.Areas.Sessions.Dtos;
using DigitalQueue.Web.Data;
using DigitalQueue.Web.Filters;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Areas.Sessions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SessionsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [AllowAnonymous]
        [ProducesResponseType(typeof(AccessTokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [HttpPatch("refresh-session", Name = nameof(RefreshSession))]
        public async Task<IActionResult> RefreshSession([FromBody] SessionTokenDto body)
        {
            var tokens = await _mediator.Send(new RefreshSessionCommand(body.Token));
            if (tokens is null)
            {
                return BadRequest();
            }
        
            return Ok(tokens);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPost("terminate-session", Name = nameof(TerminateSession))]
        public async Task<IActionResult> TerminateSession()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sessionId = User.FindFirstValue(ClaimTypesDefaults.Session);
            await _mediator.Send(new DeleteSessionCommand(currentUserId, sessionId));
            
            return NoContent();
        }
    }
}
