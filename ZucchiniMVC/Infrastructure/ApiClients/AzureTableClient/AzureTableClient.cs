using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;

namespace Zucchinimvc.Infrastructure.ApiClients.AzureTableClient
{
    public interface IAzureTableClient
    {
        TableClient GetClient(string tableName);
    }

    public class AzureTableClient : IAzureTableClient
    {
        private readonly TableServiceClient _serviceClient;

        public AzureTableClient(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AzureStorage");
            _serviceClient = new TableServiceClient(connectionString);
        }
        public TableClient GetClient(string tableName)
        {
            var client = _serviceClient.GetTableClient(tableName);
            client.CreateIfNotExists();
            return client;
        }
    }
}
