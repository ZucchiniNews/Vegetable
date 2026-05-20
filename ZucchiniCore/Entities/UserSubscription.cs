namespace ZucchiniCore.Entities
{
    public class UserSubscription
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string ProviderUserId { get; set; } = string.Empty;
        public string ProviderSubscriptionId { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public SubscriptionStatus Status { get; set; }
        public DateTime? ActivatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? PlanId { get; set; }
        // Recommended fields for Stripe lifecycle
        public DateTime? CurrentPeriodEnd { get; set; }
        public bool CancelAtPeriodEnd { get; set; }
        public DateTime? CancelledAt { get; set; }
        public User User { get; set; } = null!;
    }
}

