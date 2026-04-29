namespace ZucchiniCore.Entities;

public class BillingAccount
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string StripeCustomerId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}