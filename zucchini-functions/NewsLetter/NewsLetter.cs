
using Resend;
using SharedLib.DTOs.QueuePublisherDOTs;
using System.Text.Json;


namespace zucchini_functions.NewsLetter
{
    public class NewsLetter : INewsLetter
    {
        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
        private readonly IResend _resend;

        public NewsLetter(IResend resend)
        {
            _resend = resend;
        }



        public async Task SendEmail(string message, CancellationToken cancellationToken)
        {
            var emailFrom = Environment.GetEnvironmentVariable("Resend:FROM_EMAIL")
                ?? throw new InvalidOperationException("FROM_EMAIL not set");

            var newsletterMessage =
                JsonSerializer.Deserialize<NewsLetterQueueDto>(message);

            if (newsletterMessage is null)
                throw new InvalidOperationException("Invalid payload");

            var emailMessage = new EmailMessage
            {
                From = emailFrom,
                To = newsletterMessage.Email,
                Subject = newsletterMessage.Subject,
                HtmlBody = newsletterMessage.HtmlBody
            };

            await _resend.EmailSendAsync(emailMessage);
        }
    }
}
