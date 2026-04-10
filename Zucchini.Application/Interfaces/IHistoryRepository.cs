using Domain.Interfaces;
namespace Application.Interfaces;

public interface IHistoryRepository<T> where T : class, IHistoryRecord, new()
{
    Task UpsertAsync(T entity);
    Task<IEnumerable<T>> GetDailyHistoryAsync(string partitionKey, int days);
    Task<IEnumerable<T>> GetRecentByPartitionKeyAsync(string partitionKey, int take = 50);
    Task<IEnumerable<T>> GetAllAsync();
}