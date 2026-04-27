using Zucchinimvc.Infrastructure.ApiClients.PaymentClients.StripeGateway;
using Zucchinimvc.Infrastructure.Data;

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


        public async Task<string> CreateProviderSessionAsync(
            int subscriptionId,
            string userId,
            string providerPriceId
            )
        {
            return await _paymentClient.CreateCheckoutSessionAsync(subscriptionId, userId, providerPriceId);
        }
    }
}
