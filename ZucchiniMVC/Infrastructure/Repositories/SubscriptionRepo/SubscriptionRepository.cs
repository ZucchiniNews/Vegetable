using Infrastrcture.Repositories.SubscriptionRepo;
using Microsoft.EntityFrameworkCore;
using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.ApiClients.SubscriptionPaymentClients;
using Zucchinimvc.Infrastructure.Data;

namespace Zucchinimvc.Infrastructure.Repositories.SubscriptionRepo
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly CheckoutStripeClient _checkoutStripeClient;
        private readonly ILogger<SubscriptionRepository> _logger;

        public SubscriptionRepository(ApplicationDbContext context, CheckoutStripeClient checkoutStripeClient, ILogger<SubscriptionRepository> logger)
        {
            _context = context;
            _checkoutStripeClient = checkoutStripeClient;
            _logger = logger;
        }



        public async Task<string> CreatePaymentSessionAsync(string userId, string providerPriceId)
        {
            try
            {
                return await _checkoutStripeClient.CreateCheckoutStripeSessionAsync(userId, providerPriceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating provider session for subscriptionId, userId {UserId}", userId);
                throw;
            }
        }

        public async Task AddSubscriptionAsync(UserSubscription subscription)
        {
            try
            {
                await _context.UserSubscriptions.AddAsync(subscription);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<UserSubscription?> FindByProviderSubscriptionIdAsync(string providerSubscriptionId)
        {
            return await _context.UserSubscriptions
                .FirstOrDefaultAsync(x => x.ProviderSubscriptionId == providerSubscriptionId);
        }



        public async Task UpdateSubscriptionAsync(UserSubscription subscription)
        {
            try
            {
                _context.UserSubscriptions.Update(subscription);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<UserSubscription?> FindSubscriptionByIdAsync(int id)
        {
            try
            {
                return await _context.UserSubscriptions.FirstOrDefaultAsync(s => s.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<Plan?> FindPlanByIdAsync(int id)
        {
            try
            {
                return await _context.Plans.FirstOrDefaultAsync(st => st.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<List<Plan>> GetAllPlansAsync()
        {
            try
            {
                return await _context.Plans.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }



    }

}