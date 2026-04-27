using Infrastrcture.Repositories.SubscriptionRepo;
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

        public async Task AddSubscriptionAsync(Subscription subscription)
        {
            try
            {
                await _context.Subscriptions.AddAsync(subscription);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<Subscription?> FindSubscriptionByIdAsync(int id)
        {
            try
            {
                return await _context.Subscriptions.FirstOrDefaultAsync(s => s.Id == id);
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