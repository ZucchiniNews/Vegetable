using ZucchiniCore.Entities;

namespace Zucchinimvc.Infrastructure.ApiClients.ZucchiniStripeClient.PaymentsClients
{
    public interface IProviderPayment
    {
        Task<string> CreateCheckoutSessionAsync(string userId, SubscriptionPlan chosenPlan, string StripeCustomerId);
    }
}
