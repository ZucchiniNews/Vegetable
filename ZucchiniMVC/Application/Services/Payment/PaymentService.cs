using Infrastrcture.Repositories.SubscriptionRepo;
using ZucchiniMVC.Infrastructure.Repositories.Payment;
using Zucchinimvc.Models.ViewModels;

namespace ZucchiniMVC.Application.Services.Payment
{
    public class StripePaymentService : IPaymentService
    {
        private readonly IPaymentSubscriptionRepository _paymentRepo;
        private readonly ISubscriptionRepository _subscriptionRepo;

        public StripePaymentService(IPaymentSubscriptionRepository paymentRepo, ISubscriptionRepository subscriptionRepo)
        {
            _paymentRepo = paymentRepo;
            _subscriptionRepo = subscriptionRepo;
        }

        public async Task<PaymentSessionResult> CreatePaymentSessionAsync(int subscriptionId)
        {
            // Retrieve subscription details
            var subscription = await _subscriptionRepo.FindSubscriptionByIdAsync(subscriptionId);
            if (subscription == null)
                throw new Exception("Subscription not found");

            // Delegate session creation to the repository
            var checkoutUrl = await _paymentRepo.CreateStripeSessionAsync(subscription.Price, subscription.Id);
            return new PaymentSessionResult
            {
                CheckoutUrl = checkoutUrl,
                SessionUrl = checkoutUrl
            };
        }
    }
}


