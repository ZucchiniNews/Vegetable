using ZucchiniCore.Entities;
namespace Infrastrcture.Repositories;

public interface ISubscriptionRepository
{
    Task AddSubscriptionAsync(Subscription subscription);
    Task<SubscriptionType?> FindSubscriptionTypeByIdAsync(int id);
}