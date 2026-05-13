using ZucchiniCore.Entities;

namespace Zucchinimvc.Models.ViewModels;

public class ArticleCardViewModel
{
    public Article Article { get; set; } = new Article();
    public int ReadTimeMin { get; set; }
}
