using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;

namespace Zucchini.Infrastructure.ApiClients.AzureTableClient;

public interface IAzureTableClient { TableClient GetClient(string tableName); }

public class AzureTableClient : IAzureTableClient
{
    private readonly TableServiceClient _serviceClient;

    public AzureTableClient(IConfiguration configuration)
    {
        _serviceClient = new TableServiceClient(configuration.GetConnectionString("AzureStorage"));
    }
    public TableClient GetClient(string tableName)
    {
        var client = _serviceClient.GetTableClient(tableName);
        client.CreateIfNotExists();
        return client;
    }
}
