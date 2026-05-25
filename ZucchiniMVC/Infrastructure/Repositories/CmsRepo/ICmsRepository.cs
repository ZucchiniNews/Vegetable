using ZucchiniCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Zucchinimvc.Infrastructure.Repositories.CmsRepo
{
    public interface ICmsRepository
    {
        Task<IEnumerable<Article>> GetArticlesAsync();
        Task<Article?> GetArticleBySlugAsync(string slg);
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<IEnumerable<Article>> GetArticlesByCategoryAsync(string categorySlug);
        IQueryable<UserLikedArticle> GetUserLikedArticles();
        Task ToggleLikeAsync(int articleId, string userId);
        Task<int> GetLikeCountAsync(int articleId);
        Task<bool> IsLikedByUserAsync(int articleId, string userId);

    }
}
