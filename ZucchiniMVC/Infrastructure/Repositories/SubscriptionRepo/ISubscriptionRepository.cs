using ZucchiniCore.Entities;
namespace Infrastrcture.Repositories.SubscriptionRepo
{
    public interface ISubscriptionRepository
    {
        Task AddSubscriptionAsync(Subscription subscription);
        Task<Subscription?> FindSubscriptionByIdAsync(int id);
        Task<SubscriptionType?> FindSubscriptionTypeByIdAsync(int id);
        Task<List<SubscriptionType>> GetAllSubscriptionTypesAsync();
    }

}

