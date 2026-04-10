using Application.Interfaces;
using Domain.Interfaces;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class HistoryRepository<T> : RepositoryBase<HistoryRepository<T>>, IHistoryRepository<T> where T : class, IHistoryRecord, ITableEntity, new()
{
    private readonly TableClient _tableClient;
    public HistoryRepository(TableClient tableClient, ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
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
            logger.LogError(ex, $"Error upserting entity to {_tableClient.Name}");
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
                e => ((ITableEntity)e).PartitionKey == partitionKey && ((ITableEntity)e).RowKey.CompareTo(from) >= 0))
            {
                results.Add(entity);
            }
            return results;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.ToString());
            throw;
        }
    }

    public async Task<IEnumerable<T>> GetRecentByPartitionKeyAsync(string partitionKey, int take = 50)
    {
        try
        {
            var results = new List<T>();

            await foreach (var entity in _tableClient.QueryAsync<T>(
                e => ((ITableEntity)e).PartitionKey == partitionKey))
            {
                results.Add(entity);
            }

            return results
                .OrderBy(x => ((ITableEntity)x).RowKey)
                .TakeLast(take);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.ToString());
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
            logger.LogError(ex, $"Error fetching all entities: {ex}");
            throw;
        }
    }
}