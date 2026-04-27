using ZucchiniCore.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Zucchinimvc.Application.Services.Subscriptions;

public interface ISubscriptionService
{
    Task<Subscription> CreateSubscriptionAsync(string userId, int planId);
    Task<List<Plan>> GetAllPlansAsync();
    Task ActivateSubscriptionAsync(string subscriptionId, string stripeSubscriptionId);
    Task MarkActiveByStripeId(string stripeSubscriptionId);
    Task MarkPastDue(string stripeSubscriptionId);
    Task CancelByStripeId(string stripeSubscriptionId);
}