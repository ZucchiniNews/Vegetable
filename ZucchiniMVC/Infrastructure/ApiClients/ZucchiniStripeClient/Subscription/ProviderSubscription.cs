using Stripe;

namespace Zucchinimvc.Infrastructure.ApiClients.ZucchiniStripeClient.Subscription
{
    public class ProviderSubscription : IProviderSubscription
    {
        private readonly ZucchiniStripeClient _zucchiniStripeClient;

        public ProviderSubscription(ZucchiniStripeClient zucchiniStripeClient)
        {
            _zucchiniStripeClient = zucchiniStripeClient;
        }



        public async Task<Customer> CreateCustomerAsync(string userId)
        {
            var customerService = new CustomerService(_zucchiniStripeClient.Client);

            var customer = await customerService.CreateAsync(new CustomerCreateOptions
            {
                Metadata = new Dictionary<string, string>
                {
                    { "UserId", userId }
                }
            });

            return customer;
        }

        public async Task<Stripe.Subscription> GetSubscriptionAsync(
    string subscriptionId)
        {
            var subscriptionService =
                new SubscriptionService(_zucchiniStripeClient.Client);

            return await subscriptionService.GetAsync(subscriptionId);
        }

        public async Task<Stripe.Subscription> CancelSubscriptionAsync(
            string subscriptionId,
            bool cancelAtPeriodEnd = false)
        {
            var subscriptionService =
                new SubscriptionService(_zucchiniStripeClient.Client);

            // Cancel when billing period ends
            if (cancelAtPeriodEnd)
            {
                return await subscriptionService.UpdateAsync(
                    subscriptionId,
                    new SubscriptionUpdateOptions
                    {
                        CancelAtPeriodEnd = true
                    });
            }

            // Immediate cancellation
            return await subscriptionService.CancelAsync(subscriptionId);
        }
        public async Task<Stripe.Subscription> ReactivateSubscriptionAsync(
    string subscriptionId)
        {
            var subscriptionService =
                new SubscriptionService(_zucchiniStripeClient.Client);

            var updatedSubscription = await subscriptionService.UpdateAsync(
                subscriptionId,
                new SubscriptionUpdateOptions
                {
                    CancelAtPeriodEnd = false
                });

            return updatedSubscription;
        }
    }
}