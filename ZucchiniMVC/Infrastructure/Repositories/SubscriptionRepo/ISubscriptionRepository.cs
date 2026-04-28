using ZucchiniCore.Entities;

namespace Infrastrcture.Repositories.SubscriptionRepo
{
    public interface ISubscriptionRepository
    {
        Task<string> CreatePaymentSessionAsync(string userId, string stripePriceId);

        Task AddSubscriptionAsync(UserSubscription subscription);

        Task UpdateSubscriptionAsync(UserSubscription subscription);

        Task<Plan?> FindPlanByIdAsync(int id);

        Task<List<Plan>> GetAllPlansAsync();
    }
}

