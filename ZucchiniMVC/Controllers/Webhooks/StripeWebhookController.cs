using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using ZucchiniCore.Entities;
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
                        if (existing.Status != SubscriptionStatus.Active ||
                            existing.CancelAtPeriodEnd)
                        {
                            existing.Status = SubscriptionStatus.Active;
                            existing.CancelAtPeriodEnd = false;

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

                    await _subscriptionService.CreateZucchiniSubscription(subscription);

                    _logger.LogInformation(
                        "Subscription activated for user {UserId}. ProviderSubscriptionId: {ProviderSubscriptionId}",
                        userId, providerSubscriptionId);

                    break;
                }
            case "customer.subscription.updated":
                {
                    var stripeSubscription = stripeEvent.Data.Object as Subscription;

                    if (stripeSubscription == null)
                    {
                        _logger.LogWarning("customer.subscription.updated payload was not a valid subscription object.");
                        return Ok();
                    }

                    var providerSubscriptionId = stripeSubscription.Id;

                    if (string.IsNullOrWhiteSpace(providerSubscriptionId))
                    {
                        _logger.LogWarning("customer.subscription.updated missing subscription id.");
                        return Ok();
                    }

                    var existing = await _subscriptionService
                        .FindByProviderSubscriptionIdAsync(providerSubscriptionId);

                    if (existing == null)
                    {
                        _logger.LogWarning(
                            "No local subscription found for Stripe subscription {ProviderSubscriptionId}.",
                            providerSubscriptionId);
                        return Ok();
                    }

                    // User scheduled cancellation - still has access until period end
                    if (stripeSubscription.CancelAtPeriodEnd)
                    {
                        if (existing.Status != SubscriptionStatus.PendingCancellation)
                        {
                            _logger.LogInformation(
                                "Subscription scheduled for cancellation {ProviderSubscriptionId} for user {UserId}.",
                                providerSubscriptionId,
                                existing.UserId);

                            existing.Status = SubscriptionStatus.PendingCancellation;
                            existing.CancelAtPeriodEnd = true;
                            await _subscriptionService.UpdateSubscriptionAsync(existing);
                        }

                        return Ok();
                    }

                    // Subscription healthy again / reactivated
                    if (stripeSubscription.Status == "active")
                    {
                        if (existing.Status != SubscriptionStatus.Active)
                        {
                            _logger.LogInformation(
                                "Subscription reactivated {ProviderSubscriptionId} for user {UserId}.",
                                providerSubscriptionId,
                                existing.UserId);

                            existing.Status = SubscriptionStatus.Active;
                            existing.CancelAtPeriodEnd = false;
                            await _subscriptionService.UpdateSubscriptionAsync(existing);
                        }

                        return Ok();
                    }

                    // Failed payment - subscription in arrears
                    if (stripeSubscription.Status == "past_due")
                    {
                        if (existing.Status != SubscriptionStatus.PastDue)
                        {
                            _logger.LogWarning(
                                "Subscription payment failed {ProviderSubscriptionId} for user {UserId}.",
                                providerSubscriptionId,
                                existing.UserId);

                            existing.Status = SubscriptionStatus.PastDue;
                            await _subscriptionService.UpdateSubscriptionAsync(existing);
                        }

                        return Ok();
                    }

                    break;
                }

            case "customer.subscription.deleted":
                {
                    var stripeSubscription = stripeEvent.Data.Object as Subscription;

                    if (stripeSubscription == null)
                    {
                        _logger.LogWarning("customer.subscription.deleted payload was not a valid subscription object.");
                        return Ok();
                    }

                    var existing = await _subscriptionService
                        .FindByProviderSubscriptionIdAsync(stripeSubscription.Id);

                    if (existing == null)
                    {
                        _logger.LogWarning(
                            "No local subscription found for deletion. Stripe subscription {ProviderSubscriptionId}.",
                            stripeSubscription.Id);
                        return Ok();
                    }

                    if (existing.Status != SubscriptionStatus.Cancelled)
                    {
                        _logger.LogInformation(
                            "Subscription permanently cancelled {ProviderSubscriptionId} for user {UserId}.",
                            stripeSubscription.Id,
                            existing.UserId);

                        existing.Status = SubscriptionStatus.Cancelled;
                        existing.CancelledAt = DateTime.UtcNow;
                        await _subscriptionService.UpdateSubscriptionAsync(existing);
                    }

                    break;
                }
        }

        return Ok();
    }
}