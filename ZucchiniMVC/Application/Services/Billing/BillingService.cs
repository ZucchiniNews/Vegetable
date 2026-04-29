using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.Plans;
using Zucchinimvc.Infrastructure.ApiClients.SubscriptionPaymentClients;
using Zucchinimvc.Infrastructure.Repositories.BillingRepo;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.Application.Services.Billing
{
    public class BillingService : IBillingService
    {
        private readonly IBillingRepository _billingRepository;
        private readonly IPlanService _planService;
        private readonly CheckoutStripeClient _checkoutStripeClient;

        public BillingService(IBillingRepository billingRepository, IPlanService planService, CheckoutStripeClient checkoutStripeClient)
        {
            _billingRepository = billingRepository;
            _planService = planService;
            _checkoutStripeClient = checkoutStripeClient;
        }

        public async Task<BillingAccount> GetOrCreateStripeCustomerAsync(string userId)
        {
            var billing = await _billingRepository.GetByUserId(userId);
            if (billing != null)
                return billing;

            var customer = await _checkoutStripeClient.CreateStripeCustomerAsync(userId);

            billing = new BillingAccount
            {
                UserId = userId,
                StripeCustomerId = customer.Id,
                CreatedAt = DateTime.UtcNow
            };
            await _billingRepository.Create(billing);
            return billing;
        }

        public async Task<PaymentSessionResult> CreatePaymentSessionAsync(string userId, int planId)
        {
            var chosenPlan = await _planService.FindPlanByIdAsync(planId) ?? throw new Exception("Plan not found");
            var billingAccount = await GetOrCreateStripeCustomerAsync(userId);
            var checkoutUrl = await _checkoutStripeClient.CreateCheckoutStripeSessionAsync(userId, chosenPlan, billingAccount);
            return new PaymentSessionResult
            {
                CheckoutUrl = checkoutUrl,
                SessionUrl = checkoutUrl
            };
        }
    }
}
