using Microsoft.AspNetCore.Identity;

namespace ZucchiniCore.Entities
{
    public class User : IdentityUser
    {

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public bool NewsletterSubscribed { get; set; } = false;
        public ICollection<UserSubscription> Subscriptions { get; set; } = new List<UserSubscription>();
        public ICollection<UserLikedArticle> LikedArticles { get; set; } = new List<UserLikedArticle>();

    }

}
