using ZucchiniCore.Entities;
namespace Infrastrcture.Repositories.SubscriptionRepo
{
    public interface ISubscriptionRepository
    {
        Task AddSubscriptionAsync(Subscription subscription);
        Task<Subscription?> FindSubscriptionByIdAsync(int id);
        Task<Plan?> FindPlanByIdAsync(int id);
        Task<List<Plan>> GetAllPlansAsync();
    }

}

