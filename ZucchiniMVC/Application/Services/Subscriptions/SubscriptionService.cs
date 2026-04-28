using Infrastrcture.Repositories.SubscriptionRepo;
using ZucchiniCore.Entities;
using Zucchinimvc.Models.ViewModels;
using Zucchinimvc.Application.Services.Plans;

namespace Zucchinimvc.Application.Services.Subscriptions;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IPlanService _planService;
    private readonly ILogger<SubscriptionService> _logger;
    public SubscriptionService(ISubscriptionRepository subscriptionRepository, IPlanService planService, ILogger<SubscriptionService> logger)
    {
        _subscriptionRepository = subscriptionRepository;
        _planService = planService;
        _logger = logger;
    }

    public async Task<PaymentSessionResult> CreatePaymentSessionAsync(string userId, int planId)
    {
        var plan = await _planService.FindPlanByIdAsync(planId) ?? throw new Exception("Plan not found");
        var checkoutUrl = await _subscriptionRepository.CreatePaymentSessionAsync(userId, plan.ProviderPriceId);
        return new PaymentSessionResult
        {
            CheckoutUrl = checkoutUrl,
            SessionUrl = checkoutUrl
        };
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
}