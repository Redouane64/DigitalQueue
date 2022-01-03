using System;

using DigitalQueue.Web.Api;

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
    [ProducesResponseType(typeof(AccessTokenResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status400BadRequest)]
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
    [ProducesResponseType(typeof(AccessTokenResultDto),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorViewModel),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
    {
        var result = await _mediator.Send(command);
        if (result is null)
        {
            return BadRequest();
        }
        
        return Ok(result);
    }
}
