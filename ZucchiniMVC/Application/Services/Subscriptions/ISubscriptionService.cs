using ZucchiniCore.Entities;
using Zucchinimvc.Models.ViewModels;


namespace Zucchinimvc.Application.Services.Subscriptions;

public interface ISubscriptionService
{
    Task<PaymentSessionResult> CreatePaymentSessionAsync(string userId, int planId);
    Task<UserSubscription> CreateSubscriptionAsync(UserSubscription subscription);
    Task<UserSubscription?> FindByProviderSubscriptionIdAsync(string providerSubscriptionId);
    Task UpdateSubscriptionAsync(UserSubscription subscription);
    Task<List<Plan>> GetAllPlansAsync();
    Task<Plan?> FindPlanByIdAsync(int id);
}