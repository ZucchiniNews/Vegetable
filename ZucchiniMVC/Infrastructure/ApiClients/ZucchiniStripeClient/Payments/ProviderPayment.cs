using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.Config;

namespace Zucchinimvc.Infrastructure.ApiClients.ZucchiniStripeClient.Payments
{
    public class ProviderPayment : IProviderPayment
    {
        private readonly ZucchiniStripeClient _zucchiniStripeClient;
        private readonly StripeSettings _settings;

        public ProviderPayment(ZucchiniStripeClient zucchiniStripeClient)
        {
            _zucchiniStripeClient = zucchiniStripeClient;
            _settings = zucchiniStripeClient.Settings;
        }

        public async Task<string> CreateCheckoutSessionAsync(
            string userId,
            SubscriptionPlan chosenPlan,
            string stripeCustomerId)
        {
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },

                Mode = _settings.Mode,

                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
                {
                    new Stripe.Checkout.SessionLineItemOptions
                    {
                        Price = chosenPlan.StripePriceId,
                        Quantity = 1
                    }
                },

                SuccessUrl = $"{_settings.SuccessUrl}?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = _settings.CancelUrl,

                ClientReferenceId = userId,
                Customer = stripeCustomerId,

                Metadata = new Dictionary<string, string>
                {
                    { "userId", userId },
                    { "planId", chosenPlan.Id.ToString() }
                },

                SubscriptionData = new Stripe.Checkout.SessionSubscriptionDataOptions
                {
                    Metadata = new Dictionary<string, string>
                    {
                        { "userId", userId },
                        { "planId", chosenPlan.Id.ToString() }
                    }
                }
            };

            var service = new Stripe.Checkout.SessionService(_zucchiniStripeClient.Client);

            var session = await service.CreateAsync(options);

            return session.Url;
        }
    }
}