
using Microsoft.Extensions.Options;
using Stripe;
using Zucchinimvc.Infrastructure.Config;

namespace ZucchiniMVC.Infrastructure.ApiClients.PaymentClient
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


        public async Task<string> CreateCheckoutSessionAsync(decimal price, int subscriptionId)
        {
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
                {
                    new Stripe.Checkout.SessionLineItemOptions
                    {
                        PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(price * 100),
                            Currency = Settings.Currency ?? "usd",
                            ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Subscription #{subscriptionId}"
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = Settings.Mode ?? "payment",
                SuccessUrl = $"{Settings.SuccessUrl}?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = Settings.CancelUrl,
                Locale = Settings.Locale,
                AllowPromotionCodes = Settings.AllowPromotionCodes,
                BillingAddressCollection = Settings.BillingAddressCollection,
                CustomerEmail = string.IsNullOrWhiteSpace(Settings.CustomerEmail) ? null : Settings.CustomerEmail,
                ClientReferenceId = string.IsNullOrWhiteSpace(Settings.ClientReferenceId) ? null : Settings.ClientReferenceId,
                Metadata = Settings.Metadata,
                ExpiresAt = Settings.ExpiresAt
            };
            var service = new Stripe.Checkout.SessionService(Client);
            var session = await service.CreateAsync(options);
            return session.Url;
        }
    }
}
