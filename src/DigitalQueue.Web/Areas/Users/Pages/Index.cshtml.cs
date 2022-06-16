using DigitalQueue.Web.Areas.Users.Commands;
using DigitalQueue.Web.Areas.Users.Commands.Roles;
using DigitalQueue.Web.Areas.Users.Queries;

using MediatR;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Users.Pages;

[Authorize(
    AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, 
    Policy = "Admin"
)]
public class IndexModel : PageModel
{
    private readonly IMediator _mediator;

    public IndexModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    [TempData]
    public Boolean? PostResultMessage { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        
        var user = await _mediator.Send(new GetUserByIdQuery(id));

        if (user is null)
        {
            return NotFound();
        }

        Name = user.Name;
        Email = user.Email;
        
        return Page();
    }

    public async Task<IActionResult> OnPostRemoveRoleAsync([FromRoute] string id, [FromForm] string role)
    {
        PostResultMessage = await this._mediator.Send(new UpdateUserRolesCommand(User, id, new[] { role }, remove: true));
        return RedirectToPage(new { id });
    }

    public async Task<IActionResult> OnPostAddRoles([FromRoute] string id, [FromForm] string[] roles)
    {
        PostResultMessage = await this._mediator.Send(new UpdateUserRolesCommand(User, id, roles));
        return RedirectToPage(new { id });
    }
}
