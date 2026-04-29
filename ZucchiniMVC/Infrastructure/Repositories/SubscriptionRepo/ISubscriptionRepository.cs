using ZucchiniCore.Entities;

namespace Zucchinimvc.Infrastrcture.Repositories.SubscriptionRepo
{
    public interface ISubscriptionRepository
    {

        Task AddSubscriptionAsync(UserSubscription subscription);
        Task<UserSubscription?> FindByProviderSubscriptionIdAsync(string providerSubscriptionId);

        Task UpdateSubscriptionAsync(UserSubscription subscription);

        Task<UserSubscription?> GetLatestSubscriptionForUserAsync(string userId);
    }
}

