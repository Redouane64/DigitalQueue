using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Commands;
using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Accounts.Queries;
using DigitalQueue.Web.Filters;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

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

    [HttpPost("signin", Name = nameof(SignIn))]
    [ProducesResponseType(typeof(AccessTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignIn([FromBody]LoginCommand command)
    {
        var result = await _mediator.Send(command);
        if (result is null)
        {
            return BadRequest();
        }
        
        return Ok(result);
    }

    [HttpPost("signup", Name = nameof(SignUp))]
    [ProducesResponseType(typeof(AccessTokenDto),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignUp([FromBody] CreateAccountCommand command)
    {
        var result = await _mediator.Send(command);
        if (result is null)
        {
            return BadRequest();
        }
        
        return Ok(result);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("profile", Name = nameof(GetProfile))]
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

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("verify-email", Name = nameof(VerifyEmail))]
    public async Task<IActionResult> VerifyEmail()
    {
        _ = await this._mediator.Send(new SendEmailConfirmationCommand(User));
        return Ok();
    }
}
