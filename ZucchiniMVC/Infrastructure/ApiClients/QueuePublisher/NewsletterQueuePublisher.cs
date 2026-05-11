using Azure.Messaging.ServiceBus;
using System.Text.Json;
using ZucchiniCore.Entities;

namespace Zucchinimvc.Infrastructure.ApiClients.QueuePublisher
{
    public class NewsletterQueuePublisher
    {
        private readonly ServiceBusSender _sender;

        public NewsletterQueuePublisher(ServiceBusClient client)
        {
            _sender = client.CreateSender("newsletter-emails");
        }

        public async Task PublishAsync(NewsletterEmailJob job)
        {
            var json = JsonSerializer.Serialize(job);

            var message = new ServiceBusMessage(json)
            {
                MessageId = job.DeliveryId.ToString()
            };

            await _sender.SendMessageAsync(message);
        }
    }
}
