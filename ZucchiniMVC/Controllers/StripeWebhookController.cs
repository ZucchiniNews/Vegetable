using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Zucchinimvc.Application.Services.Subscriptions;
using Zucchinimvc.Infrastructure.Config;

[ApiController]
[Route("api/stripe/webhook")]
public class StripeWebhookController : ControllerBase
{
    private readonly ILogger<StripeWebhookController> _logger;
    private readonly ISubscriptionService _subscriptionService;
    private readonly string _webhookSecret;

    public StripeWebhookController(
        ILogger<StripeWebhookController> logger,
        ISubscriptionService subscriptionService,
        IOptions<StripeSettings> stripeOptions)
    {
        _logger = logger;
        _subscriptionService = subscriptionService;
        _webhookSecret = stripeOptions.Value.WebhookSecret;
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> StripeWebhook()
    {
        var json = await new StreamReader(Request.Body).ReadToEndAsync();

        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                _webhookSecret
            );
        }
        catch (StripeException ex)
        {
            _logger.LogWarning(ex, "Stripe webhook signature validation failed.");
            return BadRequest();
        }

        switch (stripeEvent.Type)
        {
            case "checkout.session.completed":
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    var userId = session?.ClientReferenceId;
                    var subscriptionId = session?.SubscriptionId;
                    var customerStripeId = session?.CustomerId;


                    if (userId != null && subscriptionId != null && customerStripeId != null)
                    {
                        var subscription = new ZucchiniCore.Entities.Subscription
                        {
                            UserId = userId,
                            ProviderSubscriptionId = subscriptionId,
                            ProviderUserId = customerStripeId,
                            Created = DateTime.UtcNow,
                            Status = SubscriptionStatus.Pending
                        };
                        await _subscriptionService.CreateSubscriptionAsync(subscription);
                    }
                    break;
                }

                //case "invoice.paid":
                //    {
                //        var invoice = stripeEvent.Data.Object as Stripe.Invoice;
                //        var subscriptionId = invoice?.SubscriptionId;

                //        if (subscriptionId != null)
                //        {
                //            await _subscriptionService.ActivateAsync(subscriptionId);
                //        }
                //        break;
                //    }

                //case "invoice.payment_failed":
                //    {
                //        var invoice = stripeEvent.Data.Object as Stripe.Invoice;
                //        if (invoice?.SubscriptionId != null)
                //        {
                //            await _subscriptionService.MarkPastDueAsync(invoice.SubscriptionId);
                //        }
                //        break;
                //    }

                //case "customer.subscription.deleted":
                //    {
                //        var subscription = stripeEvent.Data.Object as Stripe.Subscription;
                //        if (subscription?.Id != null)
                //        {
                //            await _subscriptionService.CancelAsync(subscription.Id);
                //        }
                //        break;
                //    }
        }

        return Ok();
    }
}