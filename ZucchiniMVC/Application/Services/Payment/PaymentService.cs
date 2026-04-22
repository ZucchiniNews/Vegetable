
using Zucchinimvc.Models.ViewModels;
using ZucchiniMVC.Infrastructure.Repositories.Payment;


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
            var subscription = await _paymentRepo.GetSubscriptionByIdAsync(subscriptionId);
            if (subscription == null)
            {
                throw new Exception("Subscription not found");
            }
            // Simulate creating a Stripe session and returning the URL
            var sessionUrl = $"https://checkout.stripe.com/pay/{subscription.Id}";

            return new PaymentSessionResult
            {
                SessionUrl = sessionUrl
            };


        }
    }
}


