using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using ZucchiniCore.Entities;

namespace Zucchinimvc.Areas.Identity.Pages.Account.Manage;

public class ConfirmEmailChangeModel : PageModel
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public ConfirmEmailChangeModel(
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public string StatusMessage { get; set; }
    public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
    {
        if (userId == null || email == null || code == null)
            return RedirectToPage("/Index");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound();

        await _userManager.ChangeEmailAsync(user, email, code);

        var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
        if (!setUserNameResult.Succeeded)
        {
            StatusMessage = "Error updating username.";
            return Page();
        }

        await _signInManager.RefreshSignInAsync(user);

        StatusMessage = "Thank you for confirming your email change.";
        return Page();
    }
}