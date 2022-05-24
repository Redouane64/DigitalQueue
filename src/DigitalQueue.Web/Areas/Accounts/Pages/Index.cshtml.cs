using MediatR;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Accounts.Pages;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "Admin")]
public class Index : PageModel
{
    private readonly IMediator _mediator;

    public Index(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task OnGet()
    {
        return Task.CompletedTask;
    }
}
