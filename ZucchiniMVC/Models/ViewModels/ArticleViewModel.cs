using ZucchiniCore.Entities;

namespace Zucchinimvc.Models.ViewModels
{
    public class ArticleViewModel
    {
        public Article Article { get; set; } = new Article();
        public bool IsSubscribed { get; set; }
        public int LikeCount { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
        public Category Category { get; set; } = new Category();
        public int ViewCount { get; set; }

    }
}