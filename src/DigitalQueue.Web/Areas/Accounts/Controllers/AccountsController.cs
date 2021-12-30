using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalQueue.Web.Areas.Accounts.Controllers;

[Route("/api/[controller]")]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
public class AccountsController : ControllerBase
{

    [HttpPost("signin", Name = nameof(SignIn))]
    public Task<IActionResult> SignIn()
    {
        return Task.FromResult(Ok());
    }

    [HttpPost("signup", Name = nameof(SignUp))]
    public Task<IActionResult> SignUp()
    {
        return Task.FromResult(Ok());
    }
}
