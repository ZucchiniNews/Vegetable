using Stripe;
using Stripe.Checkout;
using Zucchinimvc.Application.Services.Subscriptions;

namespace Zucchinimvc.Controllers.Stripe.Handlers;

public class CheckoutSessionCompletedHandler : IStripeEventHandler
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly ILogger<CheckoutSessionCompletedHandler> _logger;

    public CheckoutSessionCompletedHandler(
        ISubscriptionService subscriptionService,
        ILogger<CheckoutSessionCompletedHandler> logger)
    {
        _subscriptionService = subscriptionService;
        _logger = logger;
    }

    public async Task HandleAsync(Event stripeEvent)
    {
        var session = stripeEvent.Data.Object as Session;
        if (session?.Metadata == null)
        {
            _logger.LogWarning("checkout.session.completed: Metadata is null");
            return;
        }

        if (!session.Metadata.TryGetValue("subscriptionId", out var subscriptionIdString) ||
            !int.TryParse(subscriptionIdString, out var subscriptionId))
        {
            _logger.LogWarning("checkout.session.completed: Required metadata missing");
            return;
        }

        if (string.IsNullOrEmpty(session.SubscriptionId))
        {
            _logger.LogWarning("checkout.session.completed: Stripe subscription ID is missing");
            return;
        }


        // Create a new subscription with the Stripe subscription ID
        var subscription = new ZucchiniCore.Entities.Subscription
        {
            Id = subscriptionId,
            UserId = session.ClientReferenceId,
            Status = SubscriptionStatus.Pending,
            Created = DateTime.UtcNow
        };

        await _subscriptionService.CreateSubscriptionAsync(subscription);

        _logger.LogInformation("checkout.session.completed: Created subscription with StripeId {subscriptionId} for user {UserId}", stripeSubscriptionId, subscription.UserId);
    }
}