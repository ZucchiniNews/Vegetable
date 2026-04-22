
using Zucchinimvc.Models.ViewModels;

namespace ZucchiniMVC.Application.Services.Payment
{
    public interface IPaymentService
    {
        Task<PaymentSessionResult> CreateSubscriptionSessionAsync(int subscriptionId);
    }
}