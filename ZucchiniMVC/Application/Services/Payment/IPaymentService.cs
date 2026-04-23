
using ZucchiniCore.Entities;
using Zucchinimvc.Models.ViewModels;

namespace ZucchiniMVC.Application.Services.Payment
{
    public interface IPaymentService
    {
        Task<PaymentSessionResult> CreatePaymentSessionAsync(Subscription subscription);
    }
}