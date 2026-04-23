

namespace ZucchiniMVC.Infrastructure.Repositories.Payment
{
    public interface IPaymentSubscriptionRepository
    {
        Task<string> CreateStripeSessionAsync(decimal price, int subscriptionId);
    }
}
