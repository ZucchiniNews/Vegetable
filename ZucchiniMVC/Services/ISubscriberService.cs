using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zucchinimvc.Models;

namespace Zucchinimvc.Services;

public interface ISubscriberService
{
    Task<Subscription?> GetSubscriptionByUserIdAsync(string userId);
    Task<IEnumerable<SubscriptionType>> GetAllSubscriptionTypesAsync();
    Task SubscribeAsync(string userId, int subscriptionTypeId);
    Task UnsubscribeAsync(string userId);
}