using Microsoft.AspNetCore.Identity.UI.Services;

namespace Zucchinimvc.Services.Emails;

public class EmailSender : IEmailSender
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
}