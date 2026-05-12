using Azure.Storage.Queues;

namespace Zucchinimvc.Infrastructure.ApiClients.QueuePublisher;

public class AzureStorageQueue
{
    private readonly QueueClient _queueClient;

    public AzureStorageQueue(QueueClient queueClient)
    {
        _queueClient = queueClient;
    }

    public async Task SendMessageAsync(string message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (!await _queueClient.ExistsAsync(cancellationToken).ConfigureAwait(false))
        {
            await _queueClient.CreateIfNotExistsAsync().ConfigureAwait(false);
        }

        await _queueClient.SendMessageAsync(message, cancellationToken).ConfigureAwait(false);
    }
}
