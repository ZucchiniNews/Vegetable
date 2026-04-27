using Infrastrcture.Repositories.SubscriptionRepo;
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

    public async Task<Subscription> CreateSubscriptionAsync(string userId, int planId)
    {
        var plan = await _subscriptionRepository.FindPlanByIdAsync(planId) ?? throw new Exception("Plan not found");
        var subscription = new Subscription
        {
            PlanId = plan.Id,
            UserId = userId,
            ProviderPriceId = plan.ProviderPriceId,
            Created = DateTime.UtcNow,
            Status = SubscriptionStatus.Pending
        };
        await _subscriptionRepository.AddSubscriptionAsync(subscription);
        return subscription;
    }
    public async Task<List<Plan>> GetAllPlansAsync()
    {
        return await _subscriptionRepository.GetAllPlansAsync();
    }
}