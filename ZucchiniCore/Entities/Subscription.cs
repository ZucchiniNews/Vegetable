namespace ZucchiniCore.Entities
{
    public class Subscription
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }

        public int PlanId { get; set; }
        public Plan? Plan { get; set; }

        public decimal Price { get; set; }

        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }

        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Pending;

        public bool IsActive => Status == SubscriptionStatus.Active;
    }
}

