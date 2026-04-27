using Infrastrcture.Repositories.SubscriptionRepo;
using ZucchiniCore.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

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

    public async Task ActivateSubscriptionAsync(string subscriptionId, string stripeSubscriptionId)
    {
        if (!int.TryParse(subscriptionId, out var id))
        {
            _logger.LogWarning("Invalid subscriptionId: {SubscriptionId}", subscriptionId);
            return;
        }
        var subscription = await _subscriptionRepository.FindSubscriptionByIdAsync(id);
        if (subscription == null)
        {
            _logger.LogWarning("Subscription not found: {SubscriptionId}", subscriptionId);
            return;
        }
        subscription.Status = SubscriptionStatus.Active;
        // Optionally store stripeSubscriptionId if you have a property for it
        await _subscriptionRepository.AddSubscriptionAsync(subscription); // Replace with update if available
    }

    public async Task MarkActiveByStripeId(string stripeSubscriptionId)
    {
        // Implement lookup by Stripe subscription ID if you store it
        // For now, log as not implemented
        _logger.LogInformation("MarkActiveByStripeId called for StripeId: {StripeId}", stripeSubscriptionId);
        await Task.CompletedTask;
    }

    public async Task MarkPastDue(string stripeSubscriptionId)
    {
        // Implement lookup by Stripe subscription ID if you store it
        _logger.LogInformation("MarkPastDue called for StripeId: {StripeId}", stripeSubscriptionId);
        await Task.CompletedTask;
    }

    public async Task CancelByStripeId(string stripeSubscriptionId)
    {
        // Implement lookup by Stripe subscription ID if you store it
        _logger.LogInformation("CancelByStripeId called for StripeId: {StripeId}", stripeSubscriptionId);
        await Task.CompletedTask;
    }
}