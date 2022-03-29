using DigitalQueue.Web.Areas.Accounts.Commands;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Accounts.Pages;

[AllowAnonymous]
public class ConfirmEmail : PageModel
{
    private readonly IMediator _mediator;

    public ConfirmEmail(IMediator mediator)
    {
        _mediator = mediator;
    }

    public bool Success { get; set; }

    public async Task<IActionResult> OnGet([FromQuery]string? token, [FromQuery]string? email)
    {
        if (token is not null)
        {
            Success = await this._mediator.Send(new ConfirmUserEmailCommand(email, token));
        }

        return Page();
    }
}
