#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using ZucchiniCore.Entities; // Using your custom User entity

namespace Zucchinimvc.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<IndexModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [TempData] public string StatusMessage { get; set; }
        [TempData] public string StatusType { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            // Change Email
            [EmailAddress]
            [Display(Name = "New email")]
            public string NewEmail { get; set; }

            // Change Password
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string CurrentPassword { get; set; }

            [StringLength(100, MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
            public string ConfirmPassword { get; set; }

            public bool TwoFactorEnabled { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            Input = new InputModel
            {
                Email = await _userManager.GetEmailAsync(user),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                TwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user)
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user.");

            await LoadAsync(user);
            return Page();
        }

        // ── POST: Profile (Phone) ────────────────────────────────────────────
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (Input.PhoneNumber != user.PhoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    SetStatus("Unexpected error when setting phone number.", "danger");
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            SetStatus("Your profile has been updated.", "success");
            return RedirectToPage();
        }

        // ── POST: Change Email ───────────────────────────────────────────────
        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (string.IsNullOrEmpty(Input.NewEmail))
            {
                SetStatus("New email is required.", "danger");
                return RedirectToPage();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail == email)
            {
                SetStatus("Your email is unchanged.", "success");
                return RedirectToPage();
            }

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
            var result = await _userManager.ChangeEmailAsync(user, Input.NewEmail, token);

            if (!result.Succeeded)
            {
                SetStatus("Error changing email.", "danger");
                return RedirectToPage();
            }

            // Sync Username if your app treats them as the same
            await _userManager.SetUserNameAsync(user, Input.NewEmail);
            await _signInManager.RefreshSignInAsync(user);
            
            SetStatus("Email updated successfully.", "success");
            return RedirectToPage();
        }

        // ── POST: Change Password ────────────────────────────────────────────
        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, Input.CurrentPassword, Input.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                SetStatus("Error changing password. Ensure current password is correct.", "danger");
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            SetStatus("Your password has been changed.", "success");
            return RedirectToPage();
        }

        private void SetStatus(string message, string type)
        {
            StatusMessage = message;
            StatusType = type;
        }
    }
}