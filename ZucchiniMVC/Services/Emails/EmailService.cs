using System.Net;
using System.Net.Mail;

namespace Zucchinimvc.Services.Emails;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendAsync(string to, string subject, string htmlMessage)
    {
        try
        {
            var smtpHost = _configuration["Email:SmtpHost"];
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"]!);
            var smtpUser = _configuration["Email:SmtpUser"];
            var smtpPass = _configuration["Email:SmtpPass"];
            var fromAddress = _configuration["Email:From"];

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            var message = new MailMessage(fromAddress!, to, subject, htmlMessage)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", to);
            throw;
        }
    }

    public async Task SendConfirmationEmailAsync(string to, string confirmationLink)
    {
        var subject = "Confirm your email";
        var body = $"""
            <h2>Welcome!</h2>
            <p>Please confirm your email by clicking the link below:</p>
            <a href="{confirmationLink}">Confirm Email</a>
            """;

        await SendAsync(to, subject, body);
    }

    public async Task SendSubscriptionConfirmationAsync(string to, string userName, DateTime expires)
    {
        var subject = "Subscription Confirmed";
        var body = $"""
            <h2>Thanks, {userName}!</h2>
            <p>Your subscription is active until {expires:MMMM dd, yyyy}.</p>
            """;

        await SendAsync(to, subject, body);
    }

    public async Task SendSubscriptionExpiryReminderAsync(string to, string userName, DateTime expires)
    {
        var subject = "Your subscription is expiring soon";
        var body = $"""
            <h2>Hi {userName},</h2>
            <p>Your subscription expires on {expires:MMMM dd, yyyy}. Renew now to keep your access.</p>
            """;

        await SendAsync(to, subject, body);
    }

    public async Task SendPasswordResetAsync(string to, string resetLink)
    {
        var subject = "Reset your password";
        var body = $"""
            <h2>Password Reset</h2>
            <p>Click the link below to reset your password:</p>
            <a href="{resetLink}">Reset Password</a>
            """;

        await SendAsync(to, subject, body);
    }
}
