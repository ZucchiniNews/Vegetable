using ZucchiniCore.Entities;

namespace Zucchinimvc.Application.Services.Plans
{
    public interface IPlanService
    {
        Task<List<SubscriptionPlan>> GetAllPlansAsync();
        Task<SubscriptionPlan?> FindPlanByIdAsync(int id);
    }
}
