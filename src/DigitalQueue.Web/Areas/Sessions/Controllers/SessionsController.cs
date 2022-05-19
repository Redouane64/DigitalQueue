using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Sessions.Commands;
using DigitalQueue.Web.Areas.Sessions.Dtos;
using DigitalQueue.Web.Data;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Areas.Sessions.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SessionsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPatch("refresh-session", Name = nameof(RefreshSession))]
        public async Task<IActionResult> RefreshSession([FromBody] SessionTokenDto body)
        {
            var tokens = await _mediator.Send(new RefreshSessionCommand(body.Token));
            return tokens switch
            {
                null => StatusCode(StatusCodes.Status400BadRequest),
                _ => Ok(tokens)
            };
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPost("terminate-session", Name = nameof(TerminateSession))]
        public async Task<IActionResult> TerminateSession()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sessionSecurityStamp = User.FindFirstValue(ClaimTypesDefaults.Session);
            await _mediator.Send(new DeleteSessionCommand(currentUserId, sessionSecurityStamp));
            
            return NoContent();
        }
    }
}
