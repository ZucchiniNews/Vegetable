using Azure.Data.Tables;
using Application.Interfaces;

namespace Infrastructure.Repositories;

public class HistoryRepository<T> : IHistoryRepository<T> where T : class, ITableEntity, new()
{
    private readonly TableClient _tableClient;
    private readonly ILogger<HistoryRepository<T>> _logger;
    public HistoryRepository(TableClient tableClient, ILogger<HistoryRepository<T>> logger)
    {
        _logger = logger;
        _tableClient = tableClient;
    }

    public async Task UpsertAsync(T entity)
    {
        try
        {
            await _tableClient.UpsertEntityAsync(entity);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, $"Error upserting entity to {_tableClient.Name}");
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

    public async Task<IEnumerable<T>> GetRecentByPartitionKeyAsync(string partitionKey, int take = 50)
    {
        try
        {
            var results = new List<T>();

            await foreach (var entity in _tableClient.QueryAsync<T>(
                e => e.PartitionKey == partitionKey))
            {
                results.Add(entity);
            }

            return results
                .OrderBy(x => x.RowKey)
                .TakeLast(take);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex.ToString());
            throw;
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            var results = new List<T>();

            await foreach (var entity in _tableClient.QueryAsync<T>())
            {
                results.Add(entity);
            }
            return results;
        }
        catch (Exception ex)
        {
            _logger?.LogError($"Error fetching all entities: {ex}");
            throw;
        }
    }
}