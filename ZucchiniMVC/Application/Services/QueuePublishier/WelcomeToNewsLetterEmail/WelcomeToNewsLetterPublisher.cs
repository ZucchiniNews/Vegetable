using System.Text.Json;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.QueuePublishier.WelcomeToNewsLetterPublisher;
using Zucchinimvc.Infrastructure.ApiClients.QueuePublisher;

namespace Zucchinimvc.Application.Services.QueuePublishier.WelcomeToNewsLetterEmail;

public class WelcomeToNewsLetterPublisher
    : IWelcomeToNewsLetterPublisher
{
    private readonly ZucchiniQueueClient _queue;

    private static readonly JsonSerializerOptions SerializerOptions =
        new(JsonSerializerDefaults.Web);

    public WelcomeToNewsLetterPublisher(
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