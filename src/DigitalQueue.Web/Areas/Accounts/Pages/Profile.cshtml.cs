using System.Security.Claims;

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
    
    [TempData]
    public Boolean? EmailChangedResult { get; set; }

    public UserDto? Profile { get; set; }

    public bool IsEditable { get; set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        var profile = await _mediator.Send(new GetUserQuery(id));

        if (profile is null)
        {
            return NotFound();
        }

        IsEditable = HttpContext
            .User.FindFirstValue(ClaimTypes.NameIdentifier).Equals(id);

        Profile = profile;

        return Page();
    }

    public async Task<IActionResult> OnPost([FromRoute] string id, [FromForm] string name, [FromForm] string email)
    {
        if (name is not null)
        {
             await this._mediator.Send(new UpdateNameCommand(id, name));
        }

        if (email is not null)
        {
            PostResultMessage = EmailChangedResult = await this._mediator.Send(new UpdateEmailCommand(id, email));
        }

        return RedirectToPagePermanent("Profile", new { id });
    }

    public async Task<IActionResult> OnPostRemoveRoleAsync([FromRoute]string id, [FromForm] string role)
    {
        PostResultMessage = await this._mediator.Send(new UpdateUserRolesCommand(id, new [] { role }, remove: true));
        
        return RedirectToPage(new { id });
    }

    public async Task<IActionResult> OnPostAddRoles([FromRoute]string id, [FromForm] string[] roles)
    {
        PostResultMessage = await this._mediator.Send(new UpdateUserRolesCommand(id, roles));
        
        return RedirectToPage(new { id });
    }
}
