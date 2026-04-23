using Infrastrcture.Repositories.SubscriptionRepo;
using ZucchiniMVC.Infrastructure.Repositories.Payment;
using Zucchinimvc.Models.ViewModels;
using ZucchiniCore.Entities;

namespace ZucchiniMVC.Application.Services.Payment
{
    public class StripePaymentService : IPaymentService
    {
        private readonly IPaymentSubscriptionRepository _paymentRepo;

        public StripePaymentService(IPaymentSubscriptionRepository paymentRepo, ISubscriptionRepository subscriptionRepo)
        {
            _paymentRepo = paymentRepo;
        }

        public async Task<PaymentSessionResult> CreatePaymentSessionAsync(Subscription subscription)
        {
            if (subscription == null)
                throw new Exception("Subscription not found");
            var checkoutUrl = await _paymentRepo.CreateStripeSessionAsync(subscription.Price, subscription.Id);
            return new PaymentSessionResult
            {
                CheckoutUrl = checkoutUrl,
                SessionUrl = checkoutUrl
            };
        }
    }
}


