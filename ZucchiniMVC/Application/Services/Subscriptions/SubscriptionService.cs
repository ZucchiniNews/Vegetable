
using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.ApiClients.ZucchiniStripeClient.Subscription;
using Zucchinimvc.Infrastructure.Repositories.SubscriptionRepo;

namespace Zucchinimvc.Application.Services.Subscriptions;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IProviderSubscription _providerSubscription;
    private readonly ILogger<SubscriptionService> _logger;

    public SubscriptionService(ISubscriptionRepository subscriptionRepository, ILogger<SubscriptionService> logger, IProviderSubscription providerSubscription)
    {
        _subscriptionRepository = subscriptionRepository;
        _logger = logger;
        _providerSubscription = providerSubscription;
    }

    public async Task<UserSubscription> CreateZucchiniSubscription(UserSubscription subscription)
    {
        await _subscriptionRepository.AddSubscriptionAsync(subscription);
        return subscription;
    }


    public async Task UpdateSubscriptionAsync(UserSubscription subscription)
    {
        await _subscriptionRepository.UpdateSubscriptionAsync(subscription);
    }


    public async Task CancelProviderSubscription(UserSubscription subscription)
    {
        await _providerSubscription.CancelSubscriptionAsync(subscription.ProviderSubscriptionId, cancelAtPeriodEnd: true);

    }

    public async Task CancelZucchiniSubscription(UserSubscription subscription)
    {
        await _subscriptionRepository.CancelSubscriptionAsync(subscription);
    }




    public async Task<UserSubscription?> FindByProviderSubscriptionIdAsync(string providerSubscriptionId)
    {
        return await _subscriptionRepository.FindByProviderSubscriptionIdAsync(providerSubscriptionId);
    }



    public async Task<UserSubscription?> GetLatestSubscriptionForUserAsync(string userId)
    {
        return await _subscriptionRepository.GetLatestSubscriptionForUserAsync(userId);
    }

    public async Task<bool> UserHasActiveSubscription(string userId)
    {
        return await _subscriptionRepository.UserHasActiveSubscription(userId);
    }
    public async Task ReactivateProviderSubscription(UserSubscription subscription)
    {
        await _providerSubscription
            .ReactivateSubscriptionAsync(subscription.ProviderSubscriptionId);
    }

}
