namespace Zucchinimvc.Models.ViewModels
{

    public class SubscriptionStatusViewModel
    {
        public string PlanName { get; set; } = "No active plan";
        public SubscriptionStatus? Status { get; set; }
        public DateTime? ActivatedAt { get; set; }
        public bool HasSubscription => Status.HasValue;
    }
}
