using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using ZucchiniCore.Entities;

namespace Zucchinimvc.Services.Emails;

public class EmailSender : IEmailSender<User>, IEmailSender
{
    private readonly IEmailService _emailService;
    public EmailSender(IEmailService emailService)
    {
        _emailService = emailService;
    }
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        await _emailService.SendAsync(email, subject, htmlMessage);
    }
    public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        await _emailService.SendConfirmationEmailAsync(email, confirmationLink);
    }

    public async Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        await _emailService.SendPasswordResetAsync(email, resetLink);
    }

    public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        await _emailService.SendAsync(email, "Password Reset Code", $"Your reset code is: {resetCode}");
    }
}
