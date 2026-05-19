using Microsoft.EntityFrameworkCore;
using ZucchiniCore.Entities;

using Zucchinimvc.Infrastructure.Data;

namespace Zucchinimvc.Infrastructure.Repositories.SubscriptionRepo

{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<SubscriptionRepository> _logger;

        public SubscriptionRepository(ApplicationDbContext context, ILogger<SubscriptionRepository> logger)
        {
            _context = context;

            _logger = logger;
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


        public async Task<bool> UserHasActiveSubscription(string userId)
        {
            return await _context.UserSubscriptions
                .AnyAsync(s => s.UserId == userId && s.Status == SubscriptionStatus.Active);
        }

        public async Task<UserSubscription?> GetLatestSubscriptionForUserAsync(string userId)
        {
            return await _context.UserSubscriptions
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.Created)
                .FirstOrDefaultAsync();
        }

        public async Task CancelSubscriptionAsync(UserSubscription subscription)
        {
            try
            {
                subscription.Status = SubscriptionStatus.Cancelled;
                _context.UserSubscriptions.Update(subscription);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling subscription.");
                throw;
            }

        }

    }
}