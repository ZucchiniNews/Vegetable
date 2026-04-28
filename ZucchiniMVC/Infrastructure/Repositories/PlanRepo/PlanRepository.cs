using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.Data;

namespace Zucchinimvc.Infrastructure.Repositories.PlanRepo
{
    public class PlanRepository : IPlanRepository
    {
        private readonly ApplicationDbContext _context;
        public PlanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Plan>> GetAllPlansAsync()
        {
            return await _context.Plans.ToListAsync();
        }

        public async Task<Plan?> FindPlanByIdAsync(int id)
        {
            return await _context.Plans.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
