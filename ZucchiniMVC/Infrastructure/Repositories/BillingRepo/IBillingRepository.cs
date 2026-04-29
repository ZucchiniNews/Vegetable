using ZucchiniCore.Entities;

namespace Zucchinimvc.Infrastructure.Repositories.BillingRepo
{
    public interface IBillingRepository
    {
        Task<BillingAccount?> GetByUserId(string userId);
        Task Create(BillingAccount billingAccount);
        Task<string> CreatePaymentSessionAsync(string userId, Plan chosenPlan);
    }
}
