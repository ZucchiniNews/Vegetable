using Microsoft.EntityFrameworkCore;
using ZucchiniCore.Entities;
using Zucchinimvc.Data;

namespace Infrastrcture.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SubscriptionRepository> _logger;

    public SubscriptionRepository(ApplicationDbContext context, ILogger<SubscriptionRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Subscription?> GetByUserIdAsync(string userId)
    {
        try
        {
            return await _context.Subscriptions
                .Include(s => s.SubscriptionType)
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }

    public async Task<Subscription?> GetByIdAsync(int id)
    {
        try
        {
            return await _context.Subscriptions
                .Include(s => s.SubscriptionType)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }

    public async Task<IEnumerable<SubscriptionType>> GetAllTypesAsync()
    {
        try
        {
            return await _context.SubscriptionTypes.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }

    public async Task<SubscriptionType?> GetTypeByIdAsync(int id)
    {
        try
        {
            return await _context.SubscriptionTypes.FirstOrDefaultAsync(st => st.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }

    public async Task AddAsync(Subscription subscription)
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

    public async Task UpdateAsync(Subscription subscription)
    {
        try
        {
            _context.Subscriptions.Update(subscription);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }
}