using ZucchiniCore.Entities;

namespace Zucchinimvc.Application.Services.Plans
{
    public interface IPlanService
    {
        Task<List<Plan>> GetAllPlansAsync();
        Task<Plan?> FindPlanByIdAsync(int id);
    }
}
