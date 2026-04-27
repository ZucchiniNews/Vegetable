using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Text;
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

    [HttpPost]
    public async Task<IActionResult> Handle()
    {
        string json;
        using (var reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8))
        {
            json = await reader.ReadToEndAsync();
        }

        var stripeSignature = Request.Headers["Stripe-Signature"];
        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, _webhookSecret);
        }
        catch (StripeException ex)
        {
            _logger.LogWarning(ex, "Stripe webhook signature validation failed.");
            return BadRequest();
        }

        switch (stripeEvent.Type)
        {
            case Events.CheckoutSessionCompleted:
                var session = stripeEvent.Data.Object as Session;
                if (session?.Metadata != null &&
                    session.Metadata.TryGetValue("subscriptionId", out var subscriptionId) &&
                    session.Metadata.TryGetValue("userId", out var userId))
                {
                    var stripeSubscriptionId = session.SubscriptionId;
                    await _subscriptionService.ActivateSubscriptionAsync(subscriptionId, stripeSubscriptionId);
                }
                break;

            case Events.InvoicePaid:
                var invoicePaid = stripeEvent.Data.Object as Invoice;
                if (invoicePaid?.SubscriptionId != null)
                {
                    await _subscriptionService.MarkActiveByStripeId(invoicePaid.SubscriptionId);
                }
                break;

            case Events.InvoicePaymentFailed:
                var invoiceFailed = stripeEvent.Data.Object as Invoice;
                if (invoiceFailed?.SubscriptionId != null)
                {
                    await _subscriptionService.MarkPastDue(invoiceFailed.SubscriptionId);
                }
                break;

            case Events.CustomerSubscriptionDeleted:
                var subscriptionDeleted = stripeEvent.Data.Object as Subscription;
                if (subscriptionDeleted?.Id != null)
                {
                    await _subscriptionService.CancelByStripeId(subscriptionDeleted.Id);
                }
                break;

            default:
                _logger.LogInformation("Unhandled Stripe event type: {Type}", stripeEvent.Type);
                break;
        }

        return Ok();
    }
}