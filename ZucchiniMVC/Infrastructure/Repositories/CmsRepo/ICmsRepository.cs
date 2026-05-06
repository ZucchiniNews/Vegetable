using ZucchiniCore.Entities;
namespace Zucchinimvc.Infrastructure.Repositories.CmsRepo
{
    public interface ICmsRepository
    {
        Task<IEnumerable<Article>> GetArticlesAsync();
        Task<Article?> GetArticleBySlugAsync(string slg);
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<IEnumerable<Article>> GetArticlesByCategoryAsync(string categorySlug);
    }
}
