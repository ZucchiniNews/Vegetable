using Microsoft.Azure.Functions.Worker;
using System.Text.Json;
using Zucchinimvc.Application.Services.QueuePublishier.NewLetterQueue;
using ZucchiniMVC.Application.Services.NewsLetter;

namespace ZucchiniBackgroundJobs.Functions
{
    public class NewsletterWorkerFunction
    {
        private readonly INewsLetterService _sender;

        public NewsletterWorkerFunction(
            INewsLetterService sender)
        {
            _sender = sender;
        }

        [Function("NewsletterWorker")]
        public async Task Run(
            [QueueTrigger("newsletterqueue", Connection = "AzureStorage")]
            string messageJson,
            CancellationToken cancellationToken)
        {
            var message =
                JsonSerializer.Deserialize<
                    NewsLetterQueueMessage>(messageJson);

            if (message is null)
            {
                throw new InvalidOperationException("Newsletter queue message payload was invalid.");
            }

            await _sender.SendNewsLetterEmailAsync(
                message.Email,
                message.Subject,
                message.HtmlBody,
                cancellationToken).ConfigureAwait(false);
        }
    }
}
