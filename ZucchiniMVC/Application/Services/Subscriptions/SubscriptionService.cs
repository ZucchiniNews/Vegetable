using Infrastrcture.Repositories;
using ZucchiniCore.Entities;

namespace Zucchinimvc.Application.Services.Subscriptions;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ILogger<SubscriptionService> _logger;
    public SubscriptionService(ISubscriptionRepository subscriptionRepository, ILogger<SubscriptionService> logger)
    {
        _subscriptionRepository = subscriptionRepository;
        _logger = logger;
    }

    public async Task<Subscription> CreateSubscriptionAsync(string userId, int subscriptionTypeId)
    {
        var type = await _subscriptionRepository.FindSubscriptionTypeByIdAsync(subscriptionTypeId) ?? throw new Exception("Subscription type not found");
        var subscription = new Subscription
        {
            UserId = userId,
            SubscriptionTypeId = type.Id,
            Price = type.Price,
            Created = DateTime.UtcNow,
            // Temporary (will be corrected after payment)
            Expires = DateTime.UtcNow.AddDays(type.DurationInDays),
            Status = SubscriptionStatus.Pending
        };
        await _subscriptionRepository.AddSubscriptionAsync(subscription);
        return subscription;
    }
}