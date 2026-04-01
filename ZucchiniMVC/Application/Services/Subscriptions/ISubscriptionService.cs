using ZucchiniCore.Entities;

namespace Zucchinimvc.Services.Subscriptions;

public interface ISubscriptionService
{
    Task<Subscription?> GetActiveSubscriptionByUserIdAsync(string userId);
    Task<bool> HasActiveSubscriptionAsync(string userId);
    Task<IEnumerable<SubscriptionType>> GetAllSubscriptionTypesAsync();
    Task<SubscriptionType?> GetSubscriptionTypeByIdAsync(int id);
    Task CreateSubscriptionAsync(string userId, int subscriptionTypeId);
    Task CompletePaymentAsync(int subscriptionId);
    Task RenewSubscriptionAsync(int subscriptionId);
    Task UnsubscribeAsync(int subscriptionId);
}