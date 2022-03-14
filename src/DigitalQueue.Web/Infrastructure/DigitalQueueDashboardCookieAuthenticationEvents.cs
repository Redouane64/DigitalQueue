using System.Security.Claims;

using DigitalQueue.Web.Data.Entities;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace DigitalQueue.Web.Infrastructure;

public class DigitalQueueDashboardCookieAuthenticationEvents : CookieAuthenticationEvents
{
    private readonly UserManager<User> _userManager;

    public DigitalQueueDashboardCookieAuthenticationEvents(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        var user = await _userManager.GetUserAsync(context.Principal);

        var claims = await _userManager.GetClaimsAsync(user);
        
    }
}
