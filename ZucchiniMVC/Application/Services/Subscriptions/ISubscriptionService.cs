using ZucchiniCore.Entities;


namespace Zucchinimvc.Application.Services.Subscriptions;

public interface ISubscriptionService
{
    Task<UserSubscription> CreateZucchiniSubscription(UserSubscription subscription);
    Task UpdateSubscriptionAsync(UserSubscription subscription);
    Task CancelProviderSubscription(UserSubscription subscription);
    Task CancelZucchiniSubscription(UserSubscription subscription);
    Task<bool> UserHasActiveSubscription(string userId);
    Task<UserSubscription?> FindByProviderSubscriptionIdAsync(string providerSubscriptionId);
    Task<UserSubscription?> GetLatestSubscriptionForUserAsync(string userId);
    Task ReactivateProviderSubscription(UserSubscription subscription);

}