
using System.Text.Json;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.QueuePublishie.WelcomeToNewsLetterEmail;
using Zucchinimvc.Infrastructure.ApiClients.QueuePublisher;


namespace Zucchinimvc.Application.Services.QueuePublishier.WeeklyNewsLetterEmail
{
    public class WeeklyNewsLetterPublisher : IWeeklyNewsLetterPublisher
    {
        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
        private readonly ZucchiniQueueClient _zucchiniQueueClient;

        public WeeklyNewsLetterPublisher(ZucchiniQueueClient zucchiniQueueClient)
        {
            _zucchiniQueueClient = zucchiniQueueClient;
        }

        public async Task PublishAsync(NewsLetterQueueMessage message, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(message);

            var queueMessage = new NewsLetterQueueMessage
            {
                DeliveryId = Guid.NewGuid(),
                Email = message.Email,
                Subject = message.Subject,
                HtmlBody = message.HtmlBody
            };

            var payload = JsonSerializer.Serialize(queueMessage, SerializerOptions);
            await _zucchiniQueueClient.SendMessageAsync(payload, cancellationToken).ConfigureAwait(false);
        }
    }
}
