using Stripe;

namespace Zucchinimvc.Infrastructure.ApiClients.ZucchiniStripeClient.Subscription
{
    public interface IProviderSubscription
    {
        Task<Customer> CreateCustomerAsync(string userId);
        Task<Subscription> CancelSubscriptionAsync(string subscriptionId, bool cancelAtPeriodEnd = false);
    }
}
