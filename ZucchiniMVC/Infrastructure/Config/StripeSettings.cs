public class StripeSettings
{
    public required string SecretKey { get; set; }
    public required string PublishableKey { get; set; }
    public required string SuccessUrl { get; set; }
    public required string CancelUrl { get; set; }
}