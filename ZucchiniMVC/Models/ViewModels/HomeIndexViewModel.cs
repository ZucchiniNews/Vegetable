namespace Zucchinimvc.Models.ViewModels;

public class HomeIndexViewModel
{
    public ArticleCardViewModel? FeaturedArticle { get; set; }
    public IEnumerable<ArticleCardViewModel> LatestArticles { get; set; } = [];
    public IEnumerable<ArticleCardViewModel> EditorsChoiceArticles { get; set; } = [];
    public IEnumerable<ArticleCardViewModel> RecommendArticles { get; set; } = [];
}