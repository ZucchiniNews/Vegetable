using Microsoft.Extensions.Options;
using Resend;
using Zucchinimvc.Infrastructure.Config;

namespace Zucchinimvc.Infrastructure.ApiClients.NewsLetterEmailClient
{
    public class NewsLetterEmailClient
    {
        private readonly IResend _resend;
        private readonly NewsLetterEmailSettings _settings;

        public NewsLetterEmailClient(IResend resend, IOptions<NewsLetterEmailSettings> settings)
        {
            _resend = resend;
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string content, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_settings.FromEmail))
            {
                throw new InvalidOperationException("NewsLetterEmailSettings:FromEmail is not configured.");
            }

            var emailMessage = new EmailMessage
            {
                From = _settings.FromEmail,
                To = toEmail,
                Subject = subject,
                HtmlBody = content
            };

            await _resend.EmailSendAsync(emailMessage, cancellationToken).ConfigureAwait(false);
        }
    }
}