using ZucchiniCore.Entities;

namespace Infrastrcture.Repositories.SubscriptionRepo
{
    public interface ISubscriptionRepository
    {
        Task<string> CreatePaymentSessionAsync(string userId, string stripePriceId);

        Task AddSubscriptionAsync(UserSubscription subscription);
        Task<UserSubscription?> FindByProviderSubscriptionIdAsync(string providerSubscriptionId);

        Task UpdateSubscriptionAsync(UserSubscription subscription);

        Task<UserSubscription?> GetLatestSubscriptionForUserAsync(string userId);
    }
}

