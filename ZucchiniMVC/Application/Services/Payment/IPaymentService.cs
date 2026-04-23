
using Zucchinimvc.Models.ViewModels;

namespace ZucchiniMVC.Application.Services.Payment
{
    public interface IPaymentService
    {
        Task<PaymentSessionResult> CreatePaymentSessionAsync(int subscriptionId);
    }
}