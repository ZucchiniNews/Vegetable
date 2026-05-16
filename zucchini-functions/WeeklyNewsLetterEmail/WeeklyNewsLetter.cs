
using System.Text.Json;
using zucchini_functions.Clients.QueueClients;
using ZucchiniCore.Entities;

namespace zucchini_functions.WeeklyNewsLetterEmail
{
    public class WeeklyNewsLetter : IWeeklyNewsLetter
    {
        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
        private readonly ZucchiniQueueClient _weeklyNewsLetterQueueClient;

        public WeeklyNewsLetter(ZucchiniQueueClient weeklyNewsLetterQueueClient)
        {
            _weeklyNewsLetterQueueClient = weeklyNewsLetterQueueClient;
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
            await _weeklyNewsLetterQueueClient.SendMessageAsync(payload, cancellationToken).ConfigureAwait(false);
        }
    }
}
