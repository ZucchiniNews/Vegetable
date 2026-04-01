using ZucchiniCore.Entities;
namespace Zucchinimvc.Infrastructure.Repositories;
public interface ISubscriptionRepository
{
    Task<Subscription?> GetByUserIdAsync(string userId);
    Task<Subscription?> GetByIdAsync(int Id);
    Task<IEnumerable<SubscriptionType>> GetAllTypesAsync();
    Task<SubscriptionType?> GetTypeByIdAsync(int id);
    Task AddAsync(Subscription subscription);
    Task UpdateAsync(Subscription subscription);
}