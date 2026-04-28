namespace ZucchiniCore.Entities
{
    public class UserSubscription
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string ProviderSubscriptionId { get; set; } = string.Empty;
        public string ProviderUserId { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public SubscriptionStatus Status { get; set; }
    }
}

