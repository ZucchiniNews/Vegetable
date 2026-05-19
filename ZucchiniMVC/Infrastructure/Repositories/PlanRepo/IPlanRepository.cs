using ZucchiniCore.Entities;

namespace Zucchinimvc.Infrastructure.Repositories.PlanRepo
{
    public interface IPlanRepository
    {
        Task<List<SubscriptionPlan>> GetAllPlansAsync();
        Task<SubscriptionPlan?> FindPlanByIdAsync(int id);
    }
}
