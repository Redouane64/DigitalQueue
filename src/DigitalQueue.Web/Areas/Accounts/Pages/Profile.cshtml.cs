using DigitalQueue.Web.Areas.Accounts.Commands;
using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Areas.Accounts.Queries;

using MediatR;

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

    [TempData]
    public Boolean? PostResultMessage { get; set; }

    public UserDto? Profile { get; set; }
    
    public async Task<IActionResult> OnGetAsync(string id)
    {
        var profile = await _mediator.Send(new GetUserQuery(id));

        if (profile is null)
        {
            return NotFound();
        }

        Profile = profile;

        return Page();
    }

    public async Task<IActionResult> OnPostUpdateRoles([FromForm]string id, [FromForm] string[] roles)
    {
        PostResultMessage = await this._mediator.Send(new UpdateRoleCommand(id, roles));
        
        return RedirectToPage(new { id });
    }
}
