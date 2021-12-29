using System.Security.Claims;

using DigitalQueue.Web.Users;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Pages;

public class LoginModel : PageModel
{
    private readonly ILogger<LoginModel> _logger;
    private readonly UsersService _usersService;

    public LoginModel(ILogger<LoginModel> logger, UsersService usersService)
    {
        _logger = logger;
        _usersService = usersService;
    }

    [BindProperty]
    public string Email { get; set; } = null!;

    [BindProperty]
    public string Password { get; set; } = null!;

    [BindProperty]
    public bool RememberMe { get; set; } = true;

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostAsync()
    {
        var claims = await _usersService.AuthenticateUser(Email, Password);
        if (claims is null)
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

        return RedirectToPagePermanent("Index", new { area = "Dashboard" });
    }

}
