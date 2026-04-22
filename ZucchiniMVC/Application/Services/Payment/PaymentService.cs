
using Zucchinimvc.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using ZucchiniMVC.Infrastructure.Repositories.Payment;
using ZucchiniCore.Entities;

namespace ZucchiniMVC.Application.Services.Payment
{
    public class StripePaymentService : IPaymentService
    {
        private readonly IPaymentSubscriptionRepository _paymentRepo;

        public StripePaymentService(IPaymentSubscriptionRepository paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        public async Task<PaymentSessionResult> CreateSubscriptionSessionAsync(int subscriptionId)
        {
         
        }
    }
}


