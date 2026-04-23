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
            return await _paymentClient.CreateCheckoutSessionAsync(price, subscriptionId);
        }
    }
}
