using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Zucchinimvc.Areas.Identity.Pages.Account.Manage;

public class ManageModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ILogger<ManageModel> _logger;

    public ManageModel(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ILogger<ManageModel> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    // ── Shared page state ────────────────────────────────────────────────
    public string CurrentEmail { get; set; }
    public string CurrentDisplayName { get; set; }
    public bool HasPassword { get; set; }
    public bool TwoFactorEnabled { get; set; }
    [TempData] public string StatusMessage { get; set; }
    [TempData] public string StatusType { get; set; } // "success" | "error"

    // ── Bound forms ──────────────────────────────────────────────────────
    [BindProperty] public ChangeEmailInput EmailForm { get; set; }
    [BindProperty] public ChangePasswordInput PasswordForm { get; set; }
    [BindProperty] public ChangeNameInput NameForm { get; set; }

    // ── Input models ─────────────────────────────────────────────────────
    public class ChangeEmailInput
    {
        [Required, EmailAddress, Display(Name = "New email")]
        public string NewEmail { get; set; }
    }

    public class ChangePasswordInput
    {
        [Required, DataType(DataType.Password), Display(Name = "Current password")]
        public string CurrentPassword { get; set; }

        [Required, StringLength(100, MinimumLength = 6), DataType(DataType.Password), Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password), Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangeNameInput
    {
        [Required, StringLength(100), Display(Name = "Display name")]
        public string DisplayName { get; set; }
    }

    // ── GET ──────────────────────────────────────────────────────────────
    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        await LoadUserStateAsync(user);
        return Page();
    }

    // ── POST: Change email ───────────────────────────────────────────────
    public async Task<IActionResult> OnPostChangeEmailAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        if (!TryValidateModel(EmailForm, nameof(EmailForm)))
        {
            await LoadUserStateAsync(user);
            return Page();
        }

        if (EmailForm.NewEmail == await _userManager.GetEmailAsync(user))
        {
            SetStatus("That is already your current email.", "error");
            return RedirectToPage();
        }

        var token = await _userManager.GenerateChangeEmailTokenAsync(user, EmailForm.NewEmail);
        var result = await _userManager.ChangeEmailAsync(user, EmailForm.NewEmail, token);

        if (!result.Succeeded)
        {
            SetStatus(string.Join(" ", result.Errors.Select(e => e.Description)), "error");
            return RedirectToPage();
        }

        // Keep username in sync with email if you use email as username
        await _userManager.SetUserNameAsync(user, EmailForm.NewEmail);
        await _signInManager.RefreshSignInAsync(user);

        _logger.LogInformation("User {UserId} changed their email.", user.Id);
        SetStatus("Email updated successfully.", "success");
        return RedirectToPage();
    }

    // ── POST: Change password ────────────────────────────────────────────
    public async Task<IActionResult> OnPostChangePasswordAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        if (!TryValidateModel(PasswordForm, nameof(PasswordForm)))
        {
            await LoadUserStateAsync(user);
            return Page();
        }

        var result = await _userManager.ChangePasswordAsync(
            user, PasswordForm.CurrentPassword, PasswordForm.NewPassword);

        if (!result.Succeeded)
        {
            SetStatus(string.Join(" ", result.Errors.Select(e => e.Description)), "error");
            return RedirectToPage();
        }

        await _signInManager.RefreshSignInAsync(user);

        _logger.LogInformation("User {UserId} changed their password.", user.Id);
        SetStatus("Password updated successfully.", "success");
        return RedirectToPage();
    }

    // ── POST: Change display name ────────────────────────────────────────
    public async Task<IActionResult> OnPostChangeNameAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        if (!TryValidateModel(NameForm, nameof(NameForm)))
        {
            await LoadUserStateAsync(user);
            return Page();
        }

        // UserName is used here — swap for a custom claim or profile field if you have one
        var result = await _userManager.SetUserNameAsync(user, NameForm.DisplayName);

        if (!result.Succeeded)
        {
            SetStatus(string.Join(" ", result.Errors.Select(e => e.Description)), "error");
            return RedirectToPage();
        }

        await _signInManager.RefreshSignInAsync(user);

        _logger.LogInformation("User {UserId} changed their display name.", user.Id);
        SetStatus("Display name updated successfully.", "success");
        return RedirectToPage();
    }

    // ── Helpers ──────────────────────────────────────────────────────────
    private async Task LoadUserStateAsync(IdentityUser user)
    {
        CurrentEmail = await _userManager.GetEmailAsync(user);
        CurrentDisplayName = await _userManager.GetUserNameAsync(user);
        HasPassword = await _userManager.HasPasswordAsync(user);
        TwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
    }

    private void SetStatus(string message, string type)
    {
        StatusMessage = message;
        StatusType = type;
    }
}
