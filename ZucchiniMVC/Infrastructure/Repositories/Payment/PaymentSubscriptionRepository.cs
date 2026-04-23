using Zucchinimvc.Infrastructure.Data;
using ZucchiniMVC.Infrastructure.ApiClients.PaymentClient;

namespace ZucchiniMVC.Infrastructure.Repositories.Payment
{
    public class PaymentSubscriptionRepository : IPaymentSubscriptionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly PaymentClient _paymentClient;

        public PaymentSubscriptionRepository(ApplicationDbContext context, PaymentClient paymentClient)
        {
            _context = context;
            _paymentClient = paymentClient;
        }


        public async Task<string> CreateStripeSessionAsync(decimal price, int subscriptionId)
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
                            Currency = "usd", // Adjust as needed
                            ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Subscription #{subscriptionId}"
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = $"{_paymentClient.SuccessUrl}?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = _paymentClient.CancelUrl
            };
            var service = new Stripe.Checkout.SessionService(_paymentClient.Client);
            var session = await service.CreateAsync(options);
            return session.Url;
        }
    }
}
