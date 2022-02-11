using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Accounts.Queries;

using MediatR;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Accounts.Pages;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class ProfileModel : PageModel
{
    private readonly IMediator _mediator;

    public ProfileModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    public UserDto? Profile { get; set; }
    
    public async Task OnGetAsync()
    {
        Profile = await _mediator.Send(new GetProfileRequest(User));
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
}
