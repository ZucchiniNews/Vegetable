using ZucchiniCore.Entities;
namespace Zucchinimvc.Application.Services.Subscriptions;

public interface ISubscriptionService
{
    Task<Subscription> CreateSubscriptionAsync(string userId, int planId);
    Task<List<Plan>> GetAllPlansAsync();
}