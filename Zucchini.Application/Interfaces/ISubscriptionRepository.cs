using Domain.Entities;

namespace Application.Interfaces;

public interface ISubscriptionRepository
{
    Task<Subscription?> GetByUserIdAsync(string userId);
    Task<Subscription?> GetByIdAsync(int id);
    Task<IEnumerable<SubscriptionType>> GetAllTypesAsync();
    Task<SubscriptionType?> GetTypeByIdAsync(int id);
    Task AddAsync(Subscription subscription);
    Task UpdateAsync(Subscription subscription);
}