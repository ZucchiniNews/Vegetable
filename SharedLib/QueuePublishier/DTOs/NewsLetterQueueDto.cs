namespace SharedLib.QueuePublishier.DTOs
{
    public class NewsLetterQueueDto
    {
        public Guid DeliveryId { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public string HtmlBody { get; set; } = default!;
    }
}
