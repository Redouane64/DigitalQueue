using System.Net;
using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Commands;
using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Accounts.Queries;
using DigitalQueue.Web.Filters;

using MediatR;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Areas.Accounts.Controllers;

[Route("/api/[controller]")]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("authenticate", Name = nameof(SignIn))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SignIn([FromBody]CreateAuthenticationCodeDto body)
    {
        var result = await _mediator.Send(new CreateUserAuthenticationTokenCommand(body.Email!));
        
        if (result is null)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
        
        if (result.Created)
        {
            return StatusCode((int)HttpStatusCode.Created);
        }
        
        return Ok();
    }

    [HttpPost("verify-authentication", Name = nameof(SignUp))]
    [ProducesResponseType(typeof(TokenResult),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignUp([FromBody] VerifyAuthenticationCodeDto payload)
    {
        var result = await _mediator.Send(new VerifyUserAuthenticationTokenCommand(payload.Email, payload.Code));
        if (result is null)
        {
            return BadRequest();
        }
        
        HttpContext.Response.Headers.Add("X-Session-Id", result.Session);
        return Ok(result);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("get-profile", Name = nameof(GetProfile))]
    [ProducesResponseType(typeof(UserDto),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProfile()
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _mediator.Send(new GetUserQuery(userId));
        if (user is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest);
        }
        
        return Ok(user);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPatch("set-name", Name = nameof(SetName))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SetName([FromBody]UpdateNameDto body)
    {
        var result = await _mediator.Send(new UpdateNameCommand(body.Name));
        return result ? NoContent() : StatusCode((int)HttpStatusCode.InternalServerError);
    }

    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("request-password-reset", Name = nameof(CreatePasswordResetRequest))]
    public async Task<IActionResult> CreatePasswordResetRequest()
    {
        await this._mediator.Send(new CreatePasswordResetTokenCommand());
        return NoContent();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPatch("reset-password", Name= nameof(ChangePassword))]
    public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordDto payload)
    {
        var passwordUpdated = await this._mediator.Send(new UpdatePasswordCommand(payload.Password, payload.Token));
        return passwordUpdated ? Ok() : BadRequest();
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("request-email-change", Name= nameof(ConfirmEmail))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ConfirmEmail([FromBody] ChangeEmailDto payload)
    {
        await _mediator.Send(new SendChangeEmailCodeCommand(payload.Email));
        return NoContent();
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPatch("change-email", Name= nameof(ChangeEmail))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangeEmail([FromBody] UpdateEmailDto payload)
    {
        var emailUpdated = await _mediator.Send(new UpdateEmailCommand(payload.Token, payload.Email));
        return emailUpdated ? Ok() : StatusCode(StatusCodes.Status400BadRequest);
    }
}
