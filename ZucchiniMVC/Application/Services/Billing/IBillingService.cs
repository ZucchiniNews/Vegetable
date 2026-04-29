using ZucchiniCore.Entities;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.Application.Services.Billing
{
    public interface IBillingService
    {
        Task<BillingAccount> GetOrCreateStripeCustomerAsync(string userId);
        Task<PaymentSessionResult> CreatePaymentSessionAsync(string userId, int planId);
    }
}
