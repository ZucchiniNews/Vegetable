using Azure.Storage.Queues;
using Microsoft.Extensions.Options;
using Zucchinimvc.Infrastructure.Config;

namespace Zucchinimvc.Infrastructure.ApiClients.QueuePublisher;

public class AzureStorageQueue
{
    private readonly QueueClient _queueClient;

    public AzureStorageQueue(IOptions<QueueSettings> options)
    {
        var settings = options.Value;

        ArgumentException.ThrowIfNullOrWhiteSpace(settings.ConnectionString);
        ArgumentException.ThrowIfNullOrWhiteSpace(settings.QueueName);

        _queueClient = new QueueClient(
            settings.ConnectionString,
            settings.QueueName);
    }

    public async Task SendMessageAsync(
        string message,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        await _queueClient.CreateIfNotExistsAsync(
            cancellationToken: cancellationToken);

        await _queueClient.SendMessageAsync(
            message,
            cancellationToken);

    }
}
