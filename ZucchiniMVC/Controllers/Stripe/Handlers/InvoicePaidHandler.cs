using Stripe;
using Zucchinimvc.Application.Services.Subscriptions;

namespace Zucchinimvc.Controllers.Stripe.Handlers;

public class InvoicePaidHandler : IStripeEventHandler
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly ILogger<InvoicePaidHandler> _logger;

    public InvoicePaidHandler(
        ISubscriptionService subscriptionService,
        ILogger<InvoicePaidHandler> logger)
    {
        _subscriptionService = subscriptionService;
        _logger = logger;
    }

    public async Task HandleAsync(Event stripeEvent)
    {
        var invoice = stripeEvent.Data.Object as Invoice;
        //if (invoice == null)
        //{
        //    _logger.LogWarning("invoice.paid: Invoice object is null");
        //    return;
        //}

        //var stripeSubscriptionId = invoice.SubscriptionId;
        //if (string.IsNullOrEmpty(stripeSubscriptionId))
        //{
        //    _logger.LogWarning("invoice.paid: StripeSubscriptionId not found");
        //    return;
        //}

        //var subscription = await _subscriptionService.FindByStripeSubscriptionIdAsync(stripeSubscriptionId);
        //if (subscription == null)
        //{
        //    _logger.LogWarning("invoice.paid: Subscription not found for StripeId {StripeId}", stripeSubscriptionId);
        //    return;
        //}

        //subscription.Status = SubscriptionStatus.Active;
        //await _subscriptionService.UpdateSubscriptionAsync(subscription);

        //_logger.LogInformation("invoice.paid: Updated subscription {SubscriptionId} to Active for user {UserId}", subscription.Id, subscription.UserId);
    }
}
