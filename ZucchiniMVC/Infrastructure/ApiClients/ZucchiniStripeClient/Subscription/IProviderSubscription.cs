using Stripe;

namespace Zucchinimvc.Infrastructure.ApiClients.ZucchiniStripeClient.Subscription
{
    public interface IProviderSubscription
    {
        Task<Stripe.Subscription> GetSubscriptionAsync(string subscriptionId);
        Task<Customer> CreateCustomerAsync(string userId);
        Task<Stripe.Subscription> CancelSubscriptionAsync(string subscriptionId, bool cancelAtPeriodEnd = false);
        Task<Stripe.Subscription> ReactivateSubscriptionAsync(string providerSubscriptionId);


    }
}
