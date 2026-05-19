using ZucchiniCore.Entities;

namespace Zucchinimvc.Infrastructure.Repositories.SubscriptionRepo
{
    public interface ISubscriptionRepository
    {

        Task AddSubscriptionAsync(UserSubscription subscription);
        Task<UserSubscription?> FindByProviderSubscriptionIdAsync(string providerSubscriptionId);

        Task UpdateSubscriptionAsync(UserSubscription subscription);

        Task<UserSubscription?> GetLatestSubscriptionForUserAsync(string userId);

        Task<bool> UserHasActiveSubscription(string userId);

        Task CancelSubscriptionAsync(UserSubscription subscription);

    }
}

