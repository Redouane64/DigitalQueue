using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

using DigitalQueue.Web.Areas.Accounts.Commands;

using MediatR;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Accounts.Pages;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class ResetPassword : PageModel
{
    private readonly IMediator _mediator;

    public ResetPassword(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Required]
    [BindProperty]
    public string Code { get; set; }

    [Required]
    [BindProperty]
    public string NewPassword { get; set; }

    [Required]
    [Compare(nameof(NewPassword))]
    [BindProperty]
    public string ConfirmNewPassword { get; set; }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var success = await this._mediator.Send(new UpdatePasswordCommand(currentUserId, NewPassword, Code));

        if (!success)
        {
            ModelState.AddModelError("PasswordNotUpdated", "Something went wrong");
            return Page();
        }
        
        HttpContext.Session.SetInt32("password_updated", 1);
        return RedirectToPagePermanent("Index", new {area = "Dashboard"});
    }
}
