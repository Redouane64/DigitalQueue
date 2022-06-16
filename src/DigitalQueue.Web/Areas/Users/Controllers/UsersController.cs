using DigitalQueue.Web.Areas.Accounts.Commands.Account;
using DigitalQueue.Web.Areas.Accounts.Models;
using DigitalQueue.Web.Areas.Users.Models;
using DigitalQueue.Web.Areas.Users.Queries;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Areas.Users.Controllers;

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

    [HttpGet(Name = nameof(Get))]
    [Authorize]
    [ProducesResponseType(typeof(UserAccount), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get()
    {
        var user = await _mediator.Send(new GetUserQuery(User));
        
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPatch(Name = nameof(Update))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromBody] UpdateAccountData data)
    {
        await _mediator.Send(new UpdateAccountCommand(User, data));
        return NoContent();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("request-user-token", Name = nameof(RequestUserToken))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RequestUserToken([FromBody] CreateUserTokenData data)
    {
        await _mediator.Send(new CreateUserTokenCommand(User, data.TokenPurpose, data.Transport));
        return NoContent();
    }
}
