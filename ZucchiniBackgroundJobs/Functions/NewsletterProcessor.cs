using Microsoft.Azure.Functions.Worker;
using Resend;
using System.Text.Json;
using ZucchiniCore.Entities;

namespace ZucchiniBackgroundJobs.Functions
{
    public class NewsletterProcessor
    {
        private readonly IResend _resend;

        public NewsletterProcessor(IResend resend)
        {
            _resend = resend;
        }

        [Function("NewsletterProcessor")]
        public async Task Run(
            [ServiceBusTrigger(
            "newsletter-emails",
            Connection = "ServiceBusConnection")]
        string message)
        {
            var job = JsonSerializer.Deserialize<NewsletterEmailJob>(message);

            var email = new EmailMessage
            {
                From = "newsletter@yourdomain.com",
                To = job!.Email,
                Subject = job.Subject,
                HtmlBody = job.HtmlBody
            };

            await _resend.EmailSendAsync(email);
        }
    }
}
