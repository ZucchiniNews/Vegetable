namespace ZucchiniCore.Entities
{
    public class Subscription
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public int PlanId { get; set; }
        public string ProviderPriceId { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Pending;

        public bool IsActive => Status == SubscriptionStatus.Active;
    }
}

