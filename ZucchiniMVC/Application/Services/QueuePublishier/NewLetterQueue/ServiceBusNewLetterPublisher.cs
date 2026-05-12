using Azure.Messaging.ServiceBus;
using System.Text.Json;

namespace Zucchinimvc.Application.Services.QueuePublishier.NewLetterQueue
{
    public class ServiceBusNewLetterPublisher : INewsLetterQueuePublisher
    {
        private const string QueueName = "newsletterqueue";
        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
        private readonly ServiceBusSender _serviceBusSender;

        public ServiceBusNewLetterPublisher(ServiceBusClient serviceBusClient)
        {
            _serviceBusSender = serviceBusClient.CreateSender(QueueName);
        }

        public async Task PublishAsync(NewsLetterQueueMessage message, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(message);

            var queueMessage = new NewsLetterQueueMessage
            {
                DeliveryId = Guid.NewGuid(),
                CampaignId = Guid.NewGuid(),
                Email = message.Email,
                Subject = message.Subject,
                HtmlBody = message.HtmlBody
            };

            var serviceBusMessage = new ServiceBusMessage(JsonSerializer.Serialize(queueMessage, SerializerOptions));
            await _serviceBusSender.SendMessageAsync(serviceBusMessage, cancellationToken).ConfigureAwait(false);
        }
    }
}
