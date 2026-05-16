using Azure.Storage.Queues;

namespace zucchini_functions.Clients.QueueClients
{

    public class ZucchiniQueueClient
    {
        private readonly QueueClient _queueClient;

        public ZucchiniQueueClient(
            string connectionString,
            string queueName
            )
        {
            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.CreateIfNotExists();
        }

        public async Task SendMessageAsync(
            string message,
            CancellationToken cancellationToken = default)
        {
            await _queueClient.SendMessageAsync(
                message,
                cancellationToken);
        }
    }
}