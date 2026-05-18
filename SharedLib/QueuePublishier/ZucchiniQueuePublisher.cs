using SharedLib.Clients.QueuePublisherClient;
using SharedLib.DTOs.QueuePublisherDTOs;
using System.Text.Json;

namespace SharedLib.QueuePublishier;

public class ZucchiniQueuePublisher
    : IQueuePublisher
{
    private readonly ZucchiniQueueClient _queue;

    private static readonly JsonSerializerOptions SerializerOptions =
        new(JsonSerializerDefaults.Web);

    public ZucchiniQueuePublisher(
        ZucchiniQueueClient queue)
    {
        _queue = queue;
    }

    public async Task PublishAsync(
        NewsLetterQueueDto message,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        var queueMessage = new NewsLetterQueueDto
        {
            DeliveryId = Guid.NewGuid(),
            Email = message.Email,
            Subject = message.Subject,
            HtmlBody = message.HtmlBody
        };

        var payload = JsonSerializer.Serialize(
            queueMessage,
            SerializerOptions);

        await _queue.SendMessageAsync(payload, cancellationToken)
            .ConfigureAwait(false);
    }
}