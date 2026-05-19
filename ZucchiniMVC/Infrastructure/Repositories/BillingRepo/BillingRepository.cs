using Microsoft.EntityFrameworkCore;
using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.ApiClients.ZucchiniStripeClient.PaymentsClients;
using Zucchinimvc.Infrastructure.Data;


namespace Zucchinimvc.Infrastructure.Repositories.BillingRepo
{
    public class BillingRepository : IBillingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IProviderPayment _paymentProvider;
        private readonly ILogger<BillingRepository> _logger;
        public BillingRepository(ApplicationDbContext context, IProviderPayment paymentProvider, ILogger<BillingRepository> logger)
        {
            _context = context;
            _paymentProvider = paymentProvider;
            _logger = logger;
        }

        public async Task<BillingAccount?> GetByUserId(string userId)
        {
            return await _context.BillingAccounts.FirstOrDefaultAsync(b => b.UserId == userId);
        }

        public async Task Create(BillingAccount billingAccount)
        {
            _context.BillingAccounts.Add(billingAccount);
            await _context.SaveChangesAsync();
        }

        public async Task<string> CreatePaymentSessionAsync(string userId, SubscriptionPlan chosenPlan, string StripeCustomerId)
        {
            try
            {
                return await _paymentProvider.CreateCheckoutSessionAsync(userId, chosenPlan, StripeCustomerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating provider session for subscriptionId, userId {UserId}", userId);
                throw;
            }
        }
    }
}
