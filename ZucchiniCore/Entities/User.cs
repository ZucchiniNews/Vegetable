using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Zucchinimvc.Models
{
    public class User : IdentityUser
    {
        
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public bool IsActive { get; set; } = true;
        public bool NewsletterSubscribed { get; set; } = false;
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

        //    public ICollection<UserCategoryPreference> Preferences { get; set; }
        //    public ICollection<ArticleView> ArticleViews { get; set; }
        //    public ICollection<ArticleLike> ArticleLikes { get; set; }

    }
}
