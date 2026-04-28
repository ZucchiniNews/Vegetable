using ZucchiniCore.Entities;

namespace Infrastrcture.Repositories.SubscriptionRepo
{
    public interface ISubscriptionRepository
    {
        Task<string> CreatePaymentSessionAsync(Subscription subscription);

        Task AddSubscriptionAsync(Subscription subscription);

        Task UpdateSubscriptionAsync(Subscription subscription);

        Task<Plan?> FindPlanByIdAsync(int id);

        Task<List<Plan>> GetAllPlansAsync();
    }
}

