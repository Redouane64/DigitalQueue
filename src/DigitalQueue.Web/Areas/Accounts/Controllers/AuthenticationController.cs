using DigitalQueue.Web.Areas.Accounts.Commands.Authentication;
using DigitalQueue.Web.Areas.Accounts.Events;
using DigitalQueue.Web.Areas.Accounts.Models;
using DigitalQueue.Web.Filters;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Areas.Accounts.Controllers;


[Route("/api/[controller]")]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("authenticate", Name = nameof(Authenticate))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Authenticate(
        [FromBody] CreateAuthenticationData data, 
        [FromHeader(Name = "X-Device-Token")] string deviceToken)
    {
        var result = await _mediator.Send(new AuthenticationUserCommand(data.Email, deviceToken));
        if (result is not null)
        {
            Dictionary<string, string> eventData = new()
            {
                ["AuthenticatedUserType"] = result.Type.ToString(),
                ["DeviceToken"] = deviceToken,
            };
            await _mediator.Publish(new UserAuthenticatedEvent(result.User, eventData));
        }
        
        return Ok();
    }

    [HttpPost("verify-authentication", Name = nameof(VerifyAuthentication))]
    [ProducesResponseType(typeof(TokenResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyAuthentication([FromBody] VerifyCreateAuthenticationDataCode data)
    {
        var result = await _mediator.Send(new VerifyAuthenticationTokenCommand(data.Email, data.Code));
        
        if (result is null)
        {
            return BadRequest();
        }

        return Ok(result);
    }
}
