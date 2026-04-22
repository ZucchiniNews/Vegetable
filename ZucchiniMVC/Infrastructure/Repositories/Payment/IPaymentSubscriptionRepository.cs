

using ZucchiniCore.Entities;

namespace ZucchiniMVC.Infrastructure.Repositories.Payment
{
    public interface IPaymentSubscriptionRepository
    {
        Task<Subscription> GetSubscriptionByIdAsync(int subscriptionId);
    }
}
