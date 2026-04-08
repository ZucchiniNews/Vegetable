using ZucchiniCore.Entities;

public interface ICmsService
{
    Task<List<Article>> GetArticles();
    Task<List<Category>> GetCategories();
}