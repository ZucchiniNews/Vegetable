

namespace ZucchiniMVC.Infrastructure.Repositories.Payment
{
    public interface IPaymentSubscriptionRepository
    {
        Task<string> CreateProviderSessionAsync(
             int subscriptionId,
            string userId,
            string providerPriceId
            );
    }
}
