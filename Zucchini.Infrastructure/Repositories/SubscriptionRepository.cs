using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class SubscriptionRepository : RepositoryBase<SubscriptionRepository>, ISubscriptionRepository
{
    private readonly ApplicationDbContext _context;

    public SubscriptionRepository(ApplicationDbContext context, ILoggerFactory loggerFactory)
     : base(loggerFactory)
    {
        _context = context;
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