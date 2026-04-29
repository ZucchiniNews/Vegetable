using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.Repositories.PlanRepo;

namespace Zucchinimvc.Application.Services.Plans
{
    public class PlanService : IPlanService
    {
        private readonly IPlanRepository _planRepository;
        public PlanService(IPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task<List<Plan>> GetAllPlansAsync()
        {
            return await _planRepository.GetAllPlansAsync();
        }

        public async Task<Plan?> FindPlanByIdAsync(int id)
        {
            return await _planRepository.FindPlanByIdAsync(id);
        }
    }
}
