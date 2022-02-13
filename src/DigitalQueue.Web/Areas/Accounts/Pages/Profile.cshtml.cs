using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Accounts.Queries;

using MediatR;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Accounts.Pages;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "Admin")]
public class ProfileModel : PageModel
{
    private readonly IMediator _mediator;

    public ProfileModel(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public UserDto? Profile { get; set; }
    
    public async Task<IActionResult> OnGetAsync(string id)
    {
        var profile = await _mediator.Send(new GetProfileRequest(id));

        if (profile is null)
        {
            return NotFound();
        }

        Profile = profile;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (User.Identity is {IsAuthenticated: false})
        {
            return RedirectToPage("Login", new { area = "Accounts" });
        }

        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        return RedirectToPage("Login", new { area = "Accounts" });
    }

    public async Task<IActionResult> OnPostRolesUpdate([FromForm]string id, [FromForm] string[] roles)
    {
        // TODO: implement this.
        return RedirectToPage(new { id });
    }
}
