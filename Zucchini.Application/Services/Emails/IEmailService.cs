namespace Application.Services.Emails;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string htmlMessage);
    Task SendConfirmationEmailAsync(string to, string confirmationLink);
    Task SendSubscriptionConfirmationAsync(string to, string userName, DateTime expires);
    Task SendSubscriptionExpiryReminderAsync(string to, string userName, DateTime expires);
    Task SendPasswordResetAsync(string to, string resetLink);
}