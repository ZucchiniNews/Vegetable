namespace Zucchinimvc.Infrastructure.Config
{
    public class StripeSettings
    {
        public required string SecretKey { get; set; }
        public required string PublishableKey { get; set; }
        public required string SuccessUrl { get; set; }
        public required string CancelUrl { get; set; }
        public string? Mode { get; set; }
        public string? Currency { get; set; }
        public string? Locale { get; set; }
        public bool? AllowPromotionCodes { get; set; }
        public string? BillingAddressCollection { get; set; }
        public string? CustomerEmail { get; set; }
        public string? ClientReferenceId { get; set; }
        public Dictionary<string, string>? Metadata { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }

}
