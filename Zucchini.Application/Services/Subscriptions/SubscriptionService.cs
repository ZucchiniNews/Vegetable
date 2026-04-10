using Domain.Entities;
using Application.Interfaces;
using Application.Services.Logger;
using Microsoft.Extensions.Logging;

namespace Application.Services.Subscriptions;

public class SubscriptionService : ServiceBase<SubscriptionService>, ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public SubscriptionService(ISubscriptionRepository subscriptionRepository, ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<Subscription?> GetActiveSubscriptionByUserIdAsync(string userId)
    {
        try
        {
            var subscription = await _subscriptionRepository.GetByUserIdAsync(userId);
            if (subscription == null) return null;

            return subscription.Expires > DateTime.UtcNow && subscription.PaymentComplete
                ? subscription
                : null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get active subscription for user {UserId}", userId);
            throw;
        }
    }
    public async Task<bool> HasActiveSubscriptionAsync(string userId)
    {
        var subscription = await GetActiveSubscriptionByUserIdAsync(userId);
        return subscription != null;
    }

    public async Task<IEnumerable<SubscriptionType>> GetAllSubscriptionTypesAsync()
    {
        try
        {
            return await _subscriptionRepository.GetAllTypesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get all subscription types");
            throw;
        }
    }
    public async Task<SubscriptionType?> GetSubscriptionTypeByIdAsync(int id)
    {
        try
        {
            return await _subscriptionRepository.GetTypeByIdAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get subscription type by ID {Id}", id);
            throw;
        }
    }
    public async Task CreateSubscriptionAsync(string userId, int subscriptionTypeId)
    {
        try
        {
            var existing = await _subscriptionRepository.GetByUserIdAsync(userId);
            if (existing != null)
            {
                logger.LogWarning("User {UserId} already has a subscription. Consider renewing instead.", userId);
                throw new InvalidOperationException("User already has a subscription.");
            }
            var subscriptionType = await _subscriptionRepository.GetTypeByIdAsync(subscriptionTypeId)
                ?? throw new ArgumentException("Invalid subscription type ID.");

            var subscription = new Subscription
            {
                UserId = userId,
                SubscriptionTypeId = subscriptionTypeId,
                Price = subscriptionType.Price,
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMonths(1), // default to 1 month, adjust as needed
                PaymentComplete = false
            };
            await _subscriptionRepository.AddAsync(subscription);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create subscription for user {UserId}", userId);
            throw;
        }
    }
    public async Task CompletePaymentAsync(int subscriptionId)
    {
        try
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId)
                ?? throw new ArgumentException($"Subscription {subscriptionId} not found.");

            subscription.PaymentComplete = true;
            await _subscriptionRepository.UpdateAsync(subscription);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to complete payment for subscription {SubscriptionId}", subscriptionId);
            throw;
        }
    }
    public async Task RenewSubscriptionAsync(int subscriptionId)
    {
        try
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId)
                ?? throw new ArgumentException($"Subscription {subscriptionId} not found.");

            var baseDate = subscription.Expires > DateTime.UtcNow
             ? subscription.Expires
             : DateTime.UtcNow;

            subscription.Expires = baseDate.AddMonths(1);
            subscription.PaymentComplete = false; // reset payment status for renewal

            await _subscriptionRepository.UpdateAsync(subscription);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to renew subscription {SubscriptionId}", subscriptionId);
            throw;
        }
    }
    public async Task UnsubscribeAsync(int subscriptionId)
    {
        try
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId)
                ?? throw new ArgumentException($"Subscription {subscriptionId} not found.");

            subscription.Expires = DateTime.UtcNow; // expire immediately
            subscription.PaymentComplete = false; // mark as inactive

            await _subscriptionRepository.UpdateAsync(subscription);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to unsubscribe subscription {SubscriptionId}", subscriptionId);
            throw;
        }
    }
}