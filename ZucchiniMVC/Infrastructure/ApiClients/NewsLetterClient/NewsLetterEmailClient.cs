using Microsoft.Extensions.Options;
using Resend;
using Zucchinimvc.Infrastructure.Config;

namespace Zucchinimvc.Infrastructure.ApiClients.NewsLetterEmailClient
{
    public class NewsLetterEmailClient
    {
        private readonly NewsLetterSettings _settings;
        private readonly IResend _resend;

        public NewsLetterEmailClient(
            IOptions<NewsLetterSettings> settings,
            IResend resend
            )
        {
            _settings = settings.Value;
            _resend = resend;
        }

        public bool IsConfigured =>
            !string.IsNullOrWhiteSpace(_settings.ApiKey);

        public async Task SendEmailAsync(string toEmail, string subject, string content, CancellationToken cancellationToken)
        {
            if (!IsConfigured)
            {
                throw new InvalidOperationException("NewsLetterEmailClient is not configured properly.");
            }
            var emailMessage = new EmailMessage
            {
                From = _settings.FromEmail,
                To = toEmail,
                Subject = subject,
                HtmlBody = content
            };
            await _resend.EmailSendAsync(emailMessage);
        }
    }
}