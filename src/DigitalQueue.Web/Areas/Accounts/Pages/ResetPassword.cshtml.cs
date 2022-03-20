using System.ComponentModel.DataAnnotations;

using DigitalQueue.Web.Areas.Accounts.Commands;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Accounts.Pages;

public class ResetPassword : PageModel
{
    private readonly IMediator _mediator;

    public ResetPassword(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Required]
    public string CurrentPassword { get; set; }

    [Required]
    public string NewPassword { get; set; }

    [Required]
    [Compare(nameof(NewPassword))]
    public string ConfirmNewPassword { get; set; }

    public async Task<IActionResult> OnPost([FromQuery] string token)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _ = await this._mediator.Send(new UpdatePasswordCommand(CurrentPassword, NewPassword, ConfirmNewPassword));

        return RedirectToPagePermanent("Index", new {area = "Dashboard"});
    }
}
