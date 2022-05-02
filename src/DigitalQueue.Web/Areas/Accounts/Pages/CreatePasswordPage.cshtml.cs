using System.ComponentModel.DataAnnotations;

using DigitalQueue.Web.Data.Entities;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalQueue.Web.Areas.Accounts.Pages;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class CreatePasswordPage : PageModel
{
    private readonly UserManager<User> _userManager;

    public CreatePasswordPage(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [BindProperty]
    [Required]
    public string? Password { get; set; }

    [BindProperty]
    [Required]
    [Compare(nameof(Password))]
    public string? ConfirmPassword { get; set; }

    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPost([FromQuery] string? returnUrl)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.GetUserAsync(User);
        await _userManager.AddPasswordAsync(user, Password);

        if (returnUrl is not null)
        {
            return RedirectPermanent(returnUrl);
        }
        
        return RedirectToPage("Index", new { area = "Dashboard" });
    }
}
