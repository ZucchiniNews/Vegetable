// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.Emails;
using System.Text;

namespace Zucchinimvc.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService IEmailService;
        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            IEmailService = emailService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "current password")]
            public string CurrentPassword { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            Input = new InputModel
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            bool emailChanged = false;
            bool phoneChanged = false;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var isValid = await _userManager.CheckPasswordAsync(user, Input.CurrentPassword);
            if (!isValid)
            {
                ModelState.AddModelError("", "Incorrect password.");
                return Page();
            }

            var existingEmail = await _userManager.FindByEmailAsync(Input.Email);

            if (Input.Email != user.Email)
            {
                if (existingEmail != null && existingEmail.Id != user.Id)
                {
                    ModelState.AddModelError("Input.Email", "Email already in use.");
                    return Page();
                }

                var token = await _userManager.GenerateChangeEmailTokenAsync(user, Input.Email);

                var callbackUrl = Url.Page(
                    "/Account/Manage/ConfirmEmailChange",
                    pageHandler: null,
                    values: new
                    {
                        userId = user.Id,
                        email = Input.Email,
                        code = token
                    },
                    protocol: Request.Scheme);

                await IEmailService.SendConfirmationEmailAsync(
                    Input.Email,
                    callbackUrl);

                StatusMessage = "Confirmation link sent. Please check your email.";

                emailChanged = true;
            }

           

            if (Input.PhoneNumber != user.PhoneNumber)
            {
                var result = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to update phone number.");
                    return Page();
                }
                phoneChanged = true;
            }

            await _signInManager.RefreshSignInAsync(user);

            if (emailChanged)
            {
                StatusMessage = "Confirmation link sent to new email address. Please confirm email and login again.";
            }
            else if (phoneChanged)
            {
                StatusMessage = "Your profile has been updated.";
            }
            
            return RedirectToPage();
        }
    }
}
