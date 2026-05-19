using ZucchiniCore.Entities;
using ZucchiniCore.enums;

namespace Zucchinimvc.Models.ViewModels
{
    public class SubscriptionPlansViewModel
    {
        public List<SubscriptionPlan> Plans { get; set; } = new();
        public SubscriptionStatus? CurrentSubscriptionStatus { get; set; }
        public string? CurrentPlanName { get; set; }
    }
}
