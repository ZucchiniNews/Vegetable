using Azure.Data.Tables;
using Zucchinimvc.Repositories;

namespace Zucchinimvc.Repositories;

public class HistoryRepository<T> : IHistoryRepository<T> where T : class, ITableEntity, new()
{
    private readonly TableClient _tableClient;
    private readonly ILogger<HistoryRepository<T>> _logger;
    public HistoryRepository(IConfiguration configuration, ILogger<HistoryRepository<T>> logger, string tableName)
    {
        _logger = logger;
        _tableClient = new TableClient(
            configuration.GetConnectionString("AzureStorage"), tableName
            );
    }

    public async Task UpsertDailyAsync(T entity)
    {
        try
        {
            await _tableClient.UpsertEntityAsync(entity);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex.ToString());
            throw;
        }
    }

    public async Task<IEnumerable<T>> GetDailyHistoryAsync(string partitionKey, int days)
    {
        try
        {
            var from = DateTime.UtcNow.AddDays(-days).ToString("yyyy-MM-dd");
            var results = new List<T>();
            await foreach (var entity in _tableClient.QueryAsync<T>(
                e => e.PartitionKey == partitionKey && e.RowKey.CompareTo(from) >= 0))
            {
                results.Add(entity);
            }
            return results;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex.ToString());
            throw;
        }
    }

    public async Task<IEnumerable<T>> GetByPartitionKeyAsync(string partitionKey)
    {
        try
        {
            var results = new List<T>();

            await foreach (var entity in _tableClient.QueryAsync<T>(
                e => e.PartitionKey == partitionKey))
            {
                results.Add(entity);
            }

            return results;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex.ToString());
            throw;
        }
    }
}