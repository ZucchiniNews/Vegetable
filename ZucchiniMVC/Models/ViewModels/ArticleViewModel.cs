using ZucchiniCore.Entities;

namespace Zucchinimvc.Models.ViewModels
{
    public class ArticleViewModel
    {
        public Article Article { get; set; } = new Article();

        public bool IsSubscribed { get; set; }
    }
}