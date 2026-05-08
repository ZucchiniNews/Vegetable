using ZucchiniCore.Entities;

namespace Zucchinimvc.Models.ViewModels;

public class HomeIndexViewModel
{
    public Article? FeaturedArticle { get; set; }
    public IEnumerable<Article> LatestArticles { get; set; } = [];
    public IEnumerable<Article> EditorsChoiceArticles { get; set; } = [];
}