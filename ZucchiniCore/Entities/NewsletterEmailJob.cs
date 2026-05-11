namespace ZucchiniCore.Entities
{
    public class NewsletterEmailJob
    {
        public Guid CampaignId { get; set; }

        public Guid DeliveryId { get; set; }

        public string Email { get; set; } = default!;

        public string Subject { get; set; } = default!;

        public string HtmlBody { get; set; } = default!;
    }
}
