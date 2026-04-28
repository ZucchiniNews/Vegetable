using ZucchiniCore.Entities;
using Zucchinimvc.Models.ViewModels;


namespace Zucchinimvc.Application.Services.Subscriptions;

public interface ISubscriptionService
{
    Task<PaymentSessionResult> CreatePaymentSessionAsync(string userId, int planId);
    Task<UserSubscription> CreateSubscriptionAsync(UserSubscription subscription);
    Task<List<Plan>> GetAllPlansAsync();
    Task<Plan?> FindPlanByIdAsync(int id);

    //Task ActivateSubscriptionAsync(string stripeSubscriptionId);
    //Task MarkActiveByStripeId(string stripeSubscriptionId);
    //Task MarkPastDue(string stripeSubscriptionId);
    //Task CancelByStripeId(string stripeSubscriptionId);
    //Task<Subscription?> UpdateSubscriptionAsync(Subscription subscription);

    //Task<Subscription?> FindSubscriptionByIdAsync(int id);

}