using Azure.Data.Tables;

namespace Instrastrcture.Repositories;

public interface IHistoryRepository<T> where T : class, ITableEntity, new()
{
    Task UpsertDailyAsync(T entity);
    Task<IEnumerable<T>> GetDailyHistoryAsync(string partitionKey, int days);
    Task<IEnumerable<T>> GetRecentByPartitionKeyAsync(string partitionKey, int take = 50);
}