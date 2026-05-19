using Azure.Identity;
using Azure.Monitor.Query;

namespace Zucchinimvc.Infrastructure.ApiClients.LogQueryClient
{
    public class ZuccLogQueryClient
    {
        private readonly LogsQueryClient _client;

        public ZuccLogQueryClient()
        {
            _client = new LogsQueryClient(new DefaultAzureCredential());
        }

        public LogsQueryClient GetClient()
        {
            return _client;
        }
    }
}