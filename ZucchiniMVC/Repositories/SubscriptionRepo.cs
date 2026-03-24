using Microsoft.EntityFrameworkCore;
using Zucchinimvc.Data;

namespace Zucchinimvc.Repositories;

/*public class SubscriptionRepo : ISubscriptionRepo
{
    private readonly ApplicationDbContext _context;

    public SubscriptionRepo(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync()
    {
        return await _context.Subscriptions.ToListAsync();
    }

    public async Task<Subscription> GetSubscriptionByIdAsync(int id)
    {
        return await _context.Subscriptions.FindAsync(id);
    }

    public async Task AddSubscriptionAsync(Subscription subscription)
    {
        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSubscriptionAsync(Subscription subscription)
    {
        _context.Subscriptions.Update(subscription);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSubscriptionAsync(int id)
    {
        var subscription = await _context.Subscriptions.FindAsync(id);
        if (subscription != null)
        {
            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
        }
    }
}*/