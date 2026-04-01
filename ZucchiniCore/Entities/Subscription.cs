namespace Zucchinimvc.Models
{
    public class Subscription
    {

        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }
        public int SubscriptionTypeId { get; set; }  // Subscription type (e.g., Monthly, Yearly)
        public SubscriptionType? SubscriptionType { get; set; }
        public decimal Price { get; set; }          // Pricing stored at time of purchase
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
        public bool PaymentComplete { get; set; }
        public bool IsActive { get; set; }   // optional tracking
    }

}
