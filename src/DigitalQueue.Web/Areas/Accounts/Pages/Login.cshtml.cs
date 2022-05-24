using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Accounts.Pages;

public class LoginModel : PageModel
{
    private readonly ILogger<LoginModel> _logger;
    private readonly UserManager<User> _userManager;

    public LoginModel(ILogger<LoginModel> logger, UserManager<User> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [BindProperty]
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [BindProperty]
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [BindProperty]
    public bool RememberMe { get; set; }

    public IActionResult OnGet()
    {
        if (User.Identity!.IsAuthenticated)
        {
            return RedirectToPage("Index", new { area = "Dashboard" });
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync([FromQuery] string? returnUrl)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Verify email
        var user = await _userManager.FindByEmailAsync(Email);
        if (user is null)
        {
            ModelState.AddModelError("invalid_credentials", "Invalid credentials.");
            return Page();
        }

        // populate user claims and create cookie
        var claims = await this._userManager.GetClaimsAsync(user);

        // verify user role
        var roles = await this._userManager.GetRolesAsync(user);
        if (!roles.Contains(RoleDefaults.Administrator))
        {
            /*
             * Allow only administrator role to log-in to dashboard
             */
            ModelState.AddModelError("access_denied", "Access denied.");
            return Page();
        }

        var correct = await _userManager.CheckPasswordAsync(user, Password);
        if (!correct)
        {
            ModelState.AddModelError("invalid_credentials", "Invalid credentials.");
            return Page();
        }

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
            new AuthenticationProperties()
            {
                IsPersistent = RememberMe,
            });

        if (returnUrl is not null)
        {
            return RedirectPermanent(returnUrl);
        }

        return RedirectToPage("Index", new { area = "Dashboard" });
    }

    public async Task<IActionResult> OnPostSignOutAsync()
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return RedirectToPagePermanent("Login", new { area = "Accounts" });
        }

        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        return RedirectToPagePermanent("Login", new { area = "Accounts" });
    }

}
