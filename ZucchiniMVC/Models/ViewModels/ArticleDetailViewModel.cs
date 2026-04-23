namespace Zucchinimvc.Models.ViewModels;

using ZucchiniCore.Entities;

public class ArticleDetailViewModel
{
    public Article Article { get; set; } = null!;
    public bool IsSubscribed { get; set; }
}
