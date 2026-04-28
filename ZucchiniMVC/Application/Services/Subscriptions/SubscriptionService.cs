using Infrastrcture.Repositories.SubscriptionRepo;
using ZucchiniCore.Entities;
using Zucchinimvc.Models.ViewModels;

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




    public async Task<PaymentSessionResult> CreatePaymentSessionAsync(string userId, int planId)
    {
        var plan = await _subscriptionRepository.FindPlanByIdAsync(planId) ?? throw new Exception("Plan not found");

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






    public async Task<Plan?> FindPlanByIdAsync(int id)
    {
        return await _subscriptionRepository.FindPlanByIdAsync(id);
    }

    public async Task<List<Plan>> GetAllPlansAsync()
    {
        return await _subscriptionRepository.GetAllPlansAsync();
    }



    //public async Task ActivateSubscriptionAsync(string stripeSubscriptionId)
    //{
    //    if (!int.TryParse(stripeSubscriptionId, out var id))
    //    {
    //        _logger.LogWarning("Invalid subscriptionId: {SubscriptionId}", stripeSubscriptionId);
    //        return;
    //    }
    //    var subscription = await _subscriptionRepository.FindSubscriptionByIdAsync(id);
    //    if (subscription == null)
    //    {
    //        _logger.LogWarning("Subscription not found: {SubscriptionId}", stripeSubscriptionId);
    //        return;
    //    }
    //    subscription.Status = SubscriptionStatus.Active;
    //    await _subscriptionRepository.UpdateSubscriptionAsync(subscription);
    //}


    //public async Task MarkPastDue(string stripeSubscriptionId)
    //{
    //    if (string.IsNullOrWhiteSpace(stripeSubscriptionId))
    //    {
    //        _logger.LogWarning("MarkPastDue called with empty StripeId");
    //        return;
    //    }

    //    if (!int.TryParse(stripeSubscriptionId, out var id))
    //    {
    //        _logger.LogWarning("Invalid subscriptionId: {SubscriptionId}", stripeSubscriptionId);
    //        return;
    //    }

    //    var subscription = await _subscriptionRepository.FindSubscriptionByIdAsync(id);
    //    if (subscription == null)
    //    {
    //        _logger.LogWarning("Subscription not found for StripeId: {StripeId}", stripeSubscriptionId);
    //        return;
    //    }

    //    subscription.Status = SubscriptionStatus.PastDue;
    //    await _subscriptionRepository.UpdateSubscriptionAsync(subscription);
    //}

    //public async Task CancelByStripeId(string stripeSubscriptionId)
    //{
    //    if (string.IsNullOrWhiteSpace(stripeSubscriptionId))
    //    {
    //        _logger.LogWarning("CancelByStripeId called with empty StripeId");
    //        return;
    //    }

    //    if (!int.TryParse(stripeSubscriptionId, out var id))
    //    {
    //        _logger.LogWarning("Invalid subscriptionId: {SubscriptionId}", stripeSubscriptionId);
    //        return;
    //    }

    //    var subscription = await _subscriptionRepository.FindSubscriptionByIdAsync(id);
    //    if (subscription == null)
    //    {
    //        _logger.LogWarning("Subscription not found for StripeId: {StripeId}", stripeSubscriptionId);
    //        return;
    //    }

    //    subscription.Status = SubscriptionStatus.Cancelled;
    //    await _subscriptionRepository.UpdateSubscriptionAsync(subscription);
    //}
    //public async Task<List<Plan>> GetAllPlansAsync()
    //{
    //    return await _subscriptionRepository.GetAllPlansAsync();
    //}

    //public async Task<Subscription?> UpdateSubscriptionAsync(Subscription subscription)
    //{
    //    await _subscriptionRepository.UpdateSubscriptionAsync(subscription);
    //    return subscription;
    //}

    //public async Task<Subscription?> FindSubscriptionByIdAsync(int subscriptionId)
    //{
    //    return await _subscriptionRepository.FindSubscriptionByIdAsync(subscriptionId);
    //}

    //public async Task MarkActiveByStripeId(string stripeSubscriptionId)
    //{

    //    if (!int.TryParse(stripeSubscriptionId, out var id))
    //    {
    //        _logger.LogWarning("Invalid subscriptionId: {SubscriptionId}", stripeSubscriptionId);
    //        return;
    //    }

    //    var subscription = await _subscriptionRepository.FindSubscriptionByIdAsync(id);
    //    if (subscription == null)
    //    {
    //        _logger.LogWarning("Subscription not found for StripeId: {StripeId}", stripeSubscriptionId);
    //        return;
    //    }
    //    subscription.Status = SubscriptionStatus.Active;
    //    await _subscriptionRepository.UpdateSubscriptionAsync(subscription);
    //}

    //public async Task<Subscription?> FindBySubscriptionIdAsync(string stripeSubscriptionId)
    //{
    //    return await _subscriptionRepository.FindByStripeSubscriptionIdAsync(stripeSubscriptionId);
    //}
}