namespace SharedLib.DTOs.NewsLetterSubscriber
{
    public class NewsletterSubscriberDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool NewsletterSubscribed { get; set; }
        public bool IsActive { get; set; }

    }
}
