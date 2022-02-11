using System;

using DigitalQueue.Web.Filters;

using MediatR;
using Microsoft.AspNetCore.Mvc;

using DigitalQueue.Web.Users.Commands;
using DigitalQueue.Web.Users.Dtos;
using DigitalQueue.Web.Users.Queries;

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
    public async Task<IActionResult> SignIn([FromBody]SignInCommand command)
    {
        //throw new Exception("HAHA");
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
    public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
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
        var user = await _mediator.Send(new GetProfileRequest(User));
        if (user is null)
        {
            return BadRequest();
        }
        
        return Ok(user);
    }
}
