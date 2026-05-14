using Azure.Identity;
using Azure.Monitor.Query;

namespace Zucchinimvc.Infrastructure.ApiClients.ILogQueryClient
{
    public class LogQueryClient
    {
        private readonly LogsQueryClient _client;

        public LogQueryClient()
        {
            _client = new LogsQueryClient(new DefaultAzureCredential());
        }

        public LogsQueryClient GetClient()
        {
            return _client;
        }
    }
}