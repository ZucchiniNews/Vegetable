#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using SharedLib.DTOs.QueuePublisherDOTs;
using SharedLib.QueuePublisher;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.UsersService;

namespace Zucchinimvc.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<IndexModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IUserService _userService;
        private readonly IQueuePublisher _welcomeToNewsLetterPublisher;


        public IndexModel(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<IndexModel> logger,
        IEmailSender emailSender,
        IUserService userService,
        IQueuePublisher welcomeToNewsLetterPublisher)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _userService = userService;
            _welcomeToNewsLetterPublisher = welcomeToNewsLetterPublisher;

        }


        public string CurrentEmail { get; set; }
        public string CurrentDisplayName { get; set; }
        public bool HasPassword { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool NewsletterSubscribed { get; set; }
        [TempData] public string StatusMessage { get; set; }
        [TempData] public string StatusType { get; set; }


        // ── Bound forms ──────────────────────────────────────────────────────
        [BindProperty] public ChangeEmailInput EmailForm { get; set; } = new ChangeEmailInput();
        [BindProperty] public ChangePasswordInput PasswordForm { get; set; }
        [BindProperty] public DeleteAccountInput DeleteForm { get; set; }
        [BindProperty] public ChangePhoneInput PhoneForm { get; set; }
        [BindProperty] public NewsletterInput NewsletterForm { get; set; } = new NewsletterInput();

        private async Task LoadUserStateAsync(User user)
        {
            CurrentEmail = await _userManager.GetEmailAsync(user);
            HasPassword = await _userManager.HasPasswordAsync(user);
            TwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            NewsletterSubscribed = user.NewsletterSubscribed;
            NewsletterForm = new NewsletterInput
            {
                Subscribe = user.NewsletterSubscribed
            };
            PhoneForm = new ChangePhoneInput
            {
                NewPhoneNumber = await _userManager.GetPhoneNumberAsync(user)
            };
        }

        public UserSubscription UserSubscription { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"Unable to load user.");

            await LoadUserStateAsync(user);



            return Page();
        }



        public async Task<IActionResult> OnPostChangePhoneAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            ModelState.Clear();

            if (!TryValidateModel(PhoneForm, nameof(PhoneForm)))
            {
                await LoadUserStateAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (PhoneForm.NewPhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, PhoneForm.NewPhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    SetStatus("Unexpected error when setting phone number.", "danger");
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            SetStatus("Your phone number has been updated.", "success");
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            ModelState.Clear();

            if (!TryValidateModel(EmailForm, nameof(EmailForm)))
            {
                await LoadUserStateAsync(user);
                return Page();
            }

            var currentEmail = await _userManager.GetEmailAsync(user);
            if (EmailForm.NewEmail == currentEmail)
            {
                SetStatus("Email is already set to this address.", "info");
                return RedirectToPage();
            }

            var passwordCorrect = await _userManager.CheckPasswordAsync(user, EmailForm.CurrentPassword);
            if (!passwordCorrect)
            {
                SetStatus("Incorrect password. Email not changed.", "danger");
                return RedirectToPage();
            }

            var code = await _userManager.GenerateChangeEmailTokenAsync(user, EmailForm.NewEmail);

            await _signInManager.RefreshSignInAsync(user);

            var codeBytes = Encoding.UTF8.GetBytes(code);
            var encodedCode = WebEncoders.Base64UrlEncode(codeBytes);

            var callbackUrl = Url.Page(
                "/Account/ConfirmEmailChange",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, email = EmailForm.NewEmail, code = encodedCode },
                protocol: Request.Scheme);

            var message = $"Please confirm your email change by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";

            await _emailSender.SendEmailAsync(EmailForm.NewEmail, "Confirm your email", message);

            SetStatus("Confirmation link sent. Please check your new email.", "info");
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _userManager.ChangePasswordAsync(user, PasswordForm.CurrentPassword, PasswordForm.NewPassword);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                SetStatus("Password changed.", "success");
            }
            else
            {
                SetStatus("Incorrect current password.", "danger");
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostChangeNewsletterAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            ModelState.Clear();

            if (!TryValidateModel(NewsletterForm, nameof(NewsletterForm)))
            {
                await LoadUserStateAsync(user);
                return Page();
            }

            var wasSubscribed = user.NewsletterSubscribed;
            var subscribe = NewsletterForm.Subscribe;

            if (wasSubscribed == subscribe)
            {
                SetStatus(subscribe ? "You are already subscribed to the newsletter." : "You are already unsubscribed from the newsletter.", "info");
                return RedirectToPage();
            }

            await _userService.UpdateNewsletterPreferenceAsync(user.Id, subscribe);

            if (subscribe)
            {
                var email = await _userManager.GetEmailAsync(user);
                var message = new NewsLetterQueueDto
                {
                    Email = email,
                    Subject = "Welcome to our Newsletter!",
                    HtmlBody = "<h1>Welcome to our Newsletter!</h1><p>Thank you for subscribing.</p>"
                };

                await _welcomeToNewsLetterPublisher.PublishAsync(message, HttpContext.RequestAborted);
                SetStatus("Newsletter subscription enabled. A welcome email has been queued.", "success");
            }
            else
            {
                SetStatus("Newsletter subscription disabled.", "success");
            }

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAccountAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var passwordCorrect = await _userManager.CheckPasswordAsync(user, DeleteForm.Password);

            if (!passwordCorrect)
            {
                SetStatus("Incorrect password. Account not deleted.", "danger");
                return RedirectToPage();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return RedirectToPage("/Index");
            }
            return RedirectToPage();
        }

        public class ChangeEmailInput
        {
            [Required, EmailAddress, Display(Name = "New Email")]
            public string NewEmail { get; set; }

            [Required, DataType(DataType.Password), Display(Name = "Current Password")]
            public string CurrentPassword { get; set; }
        }

        public class ChangePasswordInput
        {
            [Required, DataType(DataType.Password), Display(Name = "Current Password")]
            public string CurrentPassword { get; set; }

            [Required, StringLength(100, MinimumLength = 6), DataType(DataType.Password), Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password), Display(Name = "Confirm New Password")]
            [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public class DeleteAccountInput
        {
            [Required, DataType(DataType.Password)]
            public string Password { get; set; }
        }
        public class ChangePhoneInput
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string NewPhoneNumber { get; set; }
        }
        public class NewsletterInput
        {
            [Display(Name = "Subscribe to newsletter")]
            public bool Subscribe { get; set; }
        }
        private void SetStatus(string message, string type)
        {
            StatusMessage = message;
            StatusType = type;
        }
    }
}