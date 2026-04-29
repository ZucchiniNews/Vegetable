using ZucchiniCore.Entities;


namespace Zucchinimvc.Application.Services.Subscriptions;

public interface ISubscriptionService
{
    Task<UserSubscription> CreateSubscriptionAsync(UserSubscription subscription);
    Task UpdateSubscriptionAsync(UserSubscription subscription);
    Task<UserSubscription?> FindByProviderSubscriptionIdAsync(string providerSubscriptionId);
    Task<UserSubscription?> GetLatestSubscriptionForUserAsync(string userId);
}