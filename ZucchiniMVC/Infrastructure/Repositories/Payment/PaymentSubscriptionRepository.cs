using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.Data;

namespace ZucchiniMVC.Infrastructure.Repositories.Payment
{
    public class PaymentSubscriptionRepository : IPaymentSubscriptionRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentSubscriptionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Subscription> GetSubscriptionByIdAsync(int subscriptionId)
        {
            return await _context.Subscriptions.FindAsync(subscriptionId);
        }

    }
}
