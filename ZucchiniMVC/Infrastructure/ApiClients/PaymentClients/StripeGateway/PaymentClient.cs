
using Microsoft.Extensions.Options;
using Stripe;
using Zucchinimvc.Infrastructure.Config;

namespace Zucchinimvc.Infrastructure.ApiClients.PaymentClients.StripeGateway
{
    public class PaymentClient
    {
        public StripeClient Client { get; }
        public StripeSettings Settings { get; }

        public PaymentClient(IOptions<StripeSettings> stripeOptions)
        {
            Settings = stripeOptions.Value;
            Client = new StripeClient(Settings.SecretKey);
        }


        public async Task<string> CreateCheckoutSessionAsync(
            int subscriptionId,
            string userId,
            string stripePriceId
            )
        {
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },

                Mode = "subscription",

                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
                {
                    new Stripe.Checkout.SessionLineItemOptions
                    {
                        Price = stripePriceId,
                        Quantity = 1
                    }
                },

                SuccessUrl = $"{Settings.SuccessUrl}?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = Settings.CancelUrl,
                ClientReferenceId = userId,
                Metadata = new Dictionary<string, string>
                {
                    { "subscriptionId", subscriptionId.ToString() },
                    { "userId", userId }
                }
            };

            var service = new Stripe.Checkout.SessionService(Client);
            var session = await service.CreateAsync(options);

            return session.Url;
        }
    }
}
