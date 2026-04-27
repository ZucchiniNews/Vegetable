using Infrastrcture.Repositories.SubscriptionRepo;
using ZucchiniCore.Entities;
using Zucchinimvc.Models.ViewModels;
using ZucchiniMVC.Infrastructure.Repositories.Payment;

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
            var checkoutUrl = await _paymentRepo.CreateProviderSessionAsync(subscription.Id, subscription.UserId, subscription.ProviderPriceId);
            return new PaymentSessionResult
            {
                CheckoutUrl = checkoutUrl,
                SessionUrl = checkoutUrl
            };
        }
    }
}


