namespace Zucchinimvc.Models
{
    public class Subscription
    {

        public int Id { get; set; }

        // Relationship to user
        public string UserId { get; set; }
        public User User { get; set; }

        // Subscription type (e.g., Monthly, Yearly)
        public int SubscriptionTypeId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }

        // Pricing (stored at time of purchase)
        public decimal Price { get; set; }

        // Dates
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }

        // Payment status
        public bool PaymentComplete { get; set; }

        // Optional tracking
        public bool IsActive { get; set; }
    }

}
