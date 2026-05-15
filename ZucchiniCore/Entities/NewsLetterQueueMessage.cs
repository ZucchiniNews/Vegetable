namespace ZucchiniCore.Entities
{
    public class NewsLetterQueueMessage
    {
        public Guid DeliveryId { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public string HtmlBody { get; set; } = default!;
    }
}
