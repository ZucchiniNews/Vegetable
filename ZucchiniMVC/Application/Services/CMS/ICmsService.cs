using ZucchiniCore.Entities;

public interface ICmsService
{
    Task<IEnumerable<Article>> GetArticles();
    Task<List<Category>> GetCategories();
}