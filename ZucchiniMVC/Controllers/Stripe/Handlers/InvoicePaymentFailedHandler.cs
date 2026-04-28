using Stripe;
using Zucchinimvc.Application.Services.Subscriptions;

namespace Zucchinimvc.Controllers.Stripe.Handlers;

public class InvoicePaymentFailedHandler : IStripeEventHandler
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly ILogger<InvoicePaymentFailedHandler> _logger;

    public InvoicePaymentFailedHandler(
        ISubscriptionService subscriptionService,
        ILogger<InvoicePaymentFailedHandler> logger)
    {
        _subscriptionService = subscriptionService;
        _logger = logger;
    }

    public async Task HandleAsync(Event stripeEvent)
    {
        var invoice = stripeEvent.Data.Object as Invoice;
        //if (invoice == null)
        //{
        //    _logger.LogWarning("invoice.payment_failed: Invoice object is null");
        //    return;
        //}

        //var stripeSubscriptionId = invoice.SubscriptionId;
        //if (string.IsNullOrEmpty(stripeSubscriptionId))
        //{
        //    _logger.LogWarning("invoice.payment_failed: StripeSubscriptionId not found");
        //    return;
        //}

        //var subscription = await _subscriptionService.FindByStripeSubscriptionIdAsync(stripeSubscriptionId);
        //if (subscription == null)
        //{
        //    _logger.LogWarning("invoice.payment_failed: Subscription not found for StripeId {StripeId}", stripeSubscriptionId);
        //    return;
        //}

        //subscription.Status = SubscriptionStatus.PastDue;
        //await _subscriptionService.UpdateSubscriptionAsync(subscription);

        //_logger.LogInformation("invoice.payment_failed: Updated subscription {SubscriptionId} to PastDue for user {UserId}", subscription.Id, subscription.UserId);
    }
}
