using System.Text.Json;
using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.ApiClients.QueuePublisher;

namespace Zucchinimvc.Application.Services.QueuePublishier.WelcomeQueue;

public class AzureStorageQueueWelcomePublisher
    : IWelcomeQueuePublisher
{
    private readonly ZucchiniQueueClient _queue;

    private static readonly JsonSerializerOptions SerializerOptions =
        new(JsonSerializerDefaults.Web);

    public AzureStorageQueueWelcomePublisher(
        ZucchiniQueueClient queue)
    {
        _queue = queue;
    }

    public async Task PublishAsync(
        NewsLetterQueueMessage message,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        var queueMessage = new NewsLetterQueueMessage
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