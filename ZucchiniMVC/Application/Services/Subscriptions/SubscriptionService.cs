
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.Plans;
using Zucchinimvc.Infrastructure.Repositories.SubscriptionRepo;

namespace Zucchinimvc.Application.Services.Subscriptions;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ILogger<SubscriptionService> _logger;
    public SubscriptionService(ISubscriptionRepository subscriptionRepository, IPlanService planService, ILogger<SubscriptionService> logger)
    {
        _subscriptionRepository = subscriptionRepository;
        _logger = logger;
    }

    public async Task<UserSubscription> CreateSubscriptionAsync(UserSubscription subscription)
    {
        await _subscriptionRepository.AddSubscriptionAsync(subscription);
        return subscription;
    }

    public async Task<UserSubscription?> FindByProviderSubscriptionIdAsync(string providerSubscriptionId)
    {
        return await _subscriptionRepository.FindByProviderSubscriptionIdAsync(providerSubscriptionId);
    }
    public async Task UpdateSubscriptionAsync(UserSubscription subscription)
    {
        await _subscriptionRepository.UpdateSubscriptionAsync(subscription);
    }

    public async Task<UserSubscription?> GetLatestSubscriptionForUserAsync(string userId)
    {
        return await _subscriptionRepository.GetLatestSubscriptionForUserAsync(userId);
    }

    public async Task<bool> UserHasActiveSubscription(string userId)
    {
        return await _subscriptionRepository.UserHasActiveSubscription(userId);
    }

    public async Task CancelSubscription(UserSubscription subscription)
    {
        subscription.Status = "canceled";
        await _subscriptionRepository.UpdateSubscriptionAsync(subscription);
    }
}