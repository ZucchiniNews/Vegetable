using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.Analytics;
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
        IAnalyticsService analyticsService,
        IOptions<StripeSettings> stripeOptions)
    {
        _logger = logger;
        _subscriptionService = subscriptionService;
        _webhookSecret = stripeOptions.Value.WebhookSecret;
    }
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
            case "invoice.paid":
                {
                    var invoice = stripeEvent.Data.Object as Invoice;
                    string? providerSubscriptionId =
                        invoice?.Parent?.SubscriptionDetails?.Subscription?.Id
                        ?? invoice?.Lines?.Data?
                            .FirstOrDefault()?
                            .Parent?
                            .SubscriptionItemDetails?
                            .Subscription;

                    string? userId = invoice?
                        .Parent?
                        .SubscriptionDetails?
                        .Metadata?
                        .TryGetValue("userId", out var uid) == true
                            ? uid
                            : null;
                    string? planId = invoice?
                        .Parent?.SubscriptionDetails?
                        .Metadata?.TryGetValue("planId", out var pid) == true
                            ? pid
                            : null;

                    var customerStripeId = invoice?.CustomerId;

                    if (string.IsNullOrEmpty(providerSubscriptionId) || string.IsNullOrEmpty(userId))
                    {
                        _logger.LogWarning(
                            "invoice.paid missing required data. SubId: {SubId}, UserId: {UserId}",
                            providerSubscriptionId, userId);

                        return Ok();
                    }

                    var existing = await _subscriptionService
                        .FindByProviderSubscriptionIdAsync(providerSubscriptionId);

                    if (existing != null)
                    {
                        if (existing.Status != SubscriptionStatus.Active)
                        {
                            existing.Status = SubscriptionStatus.Active;
                            await _subscriptionService.UpdateSubscriptionAsync(existing);
                        }

                        return Ok();
                    }

                    var subscription = new UserSubscription
                    {
                        UserId = userId,
                        ProviderSubscriptionId = providerSubscriptionId,
                        ProviderUserId = customerStripeId!,
                        Status = SubscriptionStatus.Active,
                        ActivatedAt = DateTime.UtcNow,
                        PlanId = planId
                    };

                    await _subscriptionService.CreateSubscriptionAsync(subscription);

                    break;
                }
        }

        return Ok();
    }
}