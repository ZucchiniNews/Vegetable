using Microsoft.AspNetCore.Identity;

namespace Zucchinimvc.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

        //    public ICollection<UserCategoryPreference> Preferences { get; set; }

        //    public ICollection<ArticleView> ArticleViews { get; set; }
        //    public ICollection<ArticleLike> ArticleLikes { get; set; }
        //
    }
}
