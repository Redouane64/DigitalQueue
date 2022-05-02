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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> SignIn([FromBody]CreateAuthenticationCodeDto body)
    {
        await _mediator.Send(new CreateUserAuthenticationTokenCommand(body.Email!));
        return NoContent();
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
        
        return Ok(result);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("get-profile", Name = nameof(GetProfile))]
    [ProducesResponseType(typeof(UserDto),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProfile()
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return BadRequest();
        }
        
        var user = await _mediator.Send(new GetUserQuery(userId));
        if (user is null)
        {
            return BadRequest();
        }
        
        return Ok(user);
    }

    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("request-password-reset", Name = nameof(CreatePasswordResetRequest))]
    public async Task<IActionResult> CreatePasswordResetRequest()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        _ = await this._mediator.Send(new CreatePasswordResetTokenCommand(currentUserId));
        return Ok();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPatch("reset-password", Name= nameof(ChangePassword))]
    public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordDto payload)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        _ = await this._mediator.Send(new UpdatePasswordCommand(currentUserId, payload.Password, payload.Token));
        return Ok();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPatch("change-email", Name= nameof(ChangeEmail))]
    public async Task<IActionResult> ChangeEmail([FromBody] UpdateEmailDto payload)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        _ = await _mediator.Send(new UpdateEmailCommand(currentUserId, payload.Email));
        return Ok();
    }
}
