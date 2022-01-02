using System;

using MediatR;
using Microsoft.AspNetCore.Mvc;

using DigitalQueue.Web.Users.Commands;

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
    public async Task<IActionResult> SignIn([FromBody]SignInCommand command)
    {
        var result = await _mediator.Send(command);
        if (result is null)
        {
            return BadRequest();
        }
        
        return Ok(result);
    }

    [HttpPost("signup", Name = nameof(SignUp))]
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
