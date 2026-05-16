namespace zucchini_functions.WeeklyNewsLetterEmail.DTOs
{
    public class NewsletterSubscriberDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool NewsletterSubscribed { get; set; }
        public bool IsActive { get; set; }

    }
}
