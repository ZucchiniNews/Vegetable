using ZucchiniCore.Entities;

public interface ICmsService
{
    Task<IEnumerable<Article>> GetArticles();
    Task<Article?> GetArticleBySlug(string slug);
    Task<List<Category>> GetAllCategories();
    Task<List<Article>> GetArticlesByCategory(string slug);
    Task<Article> GetFeaturedArticle();
    Task<List<Article>> GetEditorsChoice();
    Task<List<Article>> GetLatestArticles();

}