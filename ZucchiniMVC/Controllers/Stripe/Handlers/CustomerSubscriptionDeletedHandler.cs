using Stripe;
using Zucchinimvc.Application.Services.Subscriptions;

namespace Zucchinimvc.Controllers.Stripe.Handlers;

public class CustomerSubscriptionDeletedHandler : IStripeEventHandler
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly ILogger<CustomerSubscriptionDeletedHandler> _logger;

    public CustomerSubscriptionDeletedHandler(
        ISubscriptionService subscriptionService,
        ILogger<CustomerSubscriptionDeletedHandler> logger)
    {
        _subscriptionService = subscriptionService;
        _logger = logger;
    }

    public async Task HandleAsync(Event stripeEvent)
    {
        //    var stripeSubscription = stripeEvent.Data.Object as Subscription;
        //    if (stripeSubscription?.Id == null)
        //    {
        //        _logger.LogWarning("customer.subscription.deleted: Subscription object or Id is null");
        //        return;
        //    }

        //    var subscription = await _subscriptionService.FindByStripeSubscriptionIdAsync(stripeSubscription.Id);
        //    if (subscription == null)
        //    {
        //        _logger.LogWarning("customer.subscription.deleted: Subscription not found for StripeId {StripeId}", stripeSubscription.Id);
        //        return;
        //    }

        //    subscription.Status = SubscriptionStatus.Canceled;
        //    await _subscriptionService.UpdateSubscriptionAsync(subscription);

        //    _logger.LogInformation("customer.subscription.deleted: Updated subscription {SubscriptionId} to Canceled for user {UserId}", subscription.Id, subscription.UserId);
    }
}
