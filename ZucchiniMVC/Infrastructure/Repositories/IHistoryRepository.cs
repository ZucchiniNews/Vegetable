using Azure.Data.Tables;
namespace Zucchinimvc.Infrastructure.Repositories;

public interface IHistoryRepository<T> where T : class, ITableEntity, new()
{
    Task UpsertAsync(T entity);
    Task<IEnumerable<T>> GetDailyHistoryAsync(string partitionKey, int days);
    Task<IEnumerable<T>> GetRecentByPartitionKeyAsync(string partitionKey, int take = 50);
    Task<IEnumerable<T>> GetAllAsync();
}