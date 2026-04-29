using Microsoft.Extensions.Options;
using Stripe;
using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.Config;


namespace Zucchinimvc.Infrastructure.ApiClients.SubscriptionPaymentClients
{
    public class CheckoutStripeClient
    {
        public StripeClient Client { get; }
        public StripeSettings Settings { get; }
        public CheckoutStripeClient(IOptions<StripeSettings> stripeOptions)
        {
            Settings = stripeOptions.Value;
            Client = new StripeClient(Settings.SecretKey);
        }


        public async Task<string> CreateCheckoutStripeSessionAsync(string userId, ZucchiniCore.Entities.Plan chosenPlan, BillingAccount billingAccount)
        {
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "subscription",
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
                {
                    new Stripe.Checkout.SessionLineItemOptions
                    {
                        Price = chosenPlan.StripePriceId,
                        Quantity = 1
                    }
                },
                SuccessUrl = $"{Settings.SuccessUrl}?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = Settings.CancelUrl,
                ClientReferenceId = userId,
                Customer = billingAccount.StripeCustomerId,
                Metadata = new Dictionary<string, string>
                 {
                        { "userId", userId },
                        { "planId", chosenPlan.Id.ToString() }
                    },
                SubscriptionData = new Stripe.Checkout.SessionSubscriptionDataOptions
                {
                    Metadata = new Dictionary<string, string>
                    {
                        { "userId", userId }
                    }
                }
            };

            var service = new Stripe.Checkout.SessionService(Client);
            var session = await service.CreateAsync(options);

            return session.Url;
        }

        public async Task<Customer> CreateStripeCustomerAsync(string userId)
        {
            var customerService = new CustomerService(Client);
            var customer = await customerService.CreateAsync(new CustomerCreateOptions
            {
                Metadata = new Dictionary<string, string> { { "UserId", userId } }
            });
            return customer;
        }
    }
}
