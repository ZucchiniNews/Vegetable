namespace ZucchiniCore.Entities
{
    public class PaymentSubscription
    {
        public int Id { get; set; }

        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; } = null!;

        public string Provider { get; set; } = string.Empty;
        // "Stripe", "PayPal", etc.

        public string ExternalCustomerId { get; set; } = string.Empty;
        public string ExternalSubscriptionId { get; set; } = string.Empty;

        public string ExternalPriceId { get; set; } = string.Empty;
    }
}