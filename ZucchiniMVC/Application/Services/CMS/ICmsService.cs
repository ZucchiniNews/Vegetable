using ZucchiniCore.Entities;

public interface ICmsService
{
    Task<IEnumerable<Article>> GetArticles();
    Task<Article> GetArticleBySlug(string slug);
    Task<List<Category>> GetCategories();
}