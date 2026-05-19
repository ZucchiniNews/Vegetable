using Stripe;

namespace Zucchinimvc.Infrastructure.ApiClients.ZucchiniStripeClient.SubscriptionClients
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

        public async Task<Subscription> CancelSubscriptionAsync(
            string subscriptionId,
            bool cancelAtPeriodEnd = false
            )
        {
            var subscriptionService = new SubscriptionService(_zucchiniStripeClient.Client);

            if (cancelAtPeriodEnd)
            {
                var updatedSubscription = await subscriptionService.UpdateAsync(
                    subscriptionId,
                    new SubscriptionUpdateOptions
                    {
                        CancelAtPeriodEnd = true
                    });

                return updatedSubscription;
            }
            // Immediate cancellation
            var canceledSubscription = await subscriptionService.CancelAsync(
                subscriptionId,
                null);

            return canceledSubscription;
        }
    }
}