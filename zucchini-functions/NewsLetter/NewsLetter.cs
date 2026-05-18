
using Microsoft.Extensions.Logging;
using Resend;
using SharedLib.QueuePublishier.DTOs;
using System.Text.Json;


namespace zucchini_functions.NewsLetter
{
    public class NewsLetter : INewsLetter
    {
        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
        private readonly IResend _resend;
        private readonly ILogger<NewsLetter> _logger;


        public NewsLetter(IResend resend, ILogger<NewsLetter> logger)
        {
            _resend = resend;
            _logger = logger;
        }



        public async Task SendEmail(string message, CancellationToken cancellationToken)
        {
            var emailFrom = Environment.GetEnvironmentVariable("FROM_EMAIL")
                ?? throw new InvalidOperationException("FROM_EMAIL not set");

            var newsletterMessage =
                JsonSerializer.Deserialize<NewsLetterQueueDto>(message, SerializerOptions);

            if (newsletterMessage is null)
                throw new InvalidOperationException("Invalid payload");

            if (string.IsNullOrWhiteSpace(newsletterMessage.Email))
                throw new InvalidOperationException("Email is missing");

            _logger.LogInformation("Email = {Email}", newsletterMessage.Email);

            var emailMessage = new EmailMessage
            {
                From = emailFrom,
                To = new[] { newsletterMessage.Email },
                Subject = newsletterMessage.Subject,
                HtmlBody = newsletterMessage.HtmlBody
            };

            await _resend.EmailSendAsync(emailMessage);
        }
    }
}
