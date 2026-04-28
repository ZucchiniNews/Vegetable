using ZucchiniCore.Entities;

namespace Zucchinimvc.Infrastructure.Repositories.PlanRepo
{
    public interface IPlanRepository
    {
        Task<List<Plan>> GetAllPlansAsync();
        Task<Plan?> FindPlanByIdAsync(int id);
    }
}
