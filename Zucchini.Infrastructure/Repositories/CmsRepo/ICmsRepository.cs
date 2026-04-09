using Zucchini.Domain.Entities;

namespace Zucchini.Infrastructure.Repositories.CmsRepo;

public interface ICmsRepository
{
    Task<IEnumerable<Article>> GetArticlesAsync();
    Task<IEnumerable<Category>> GetCategoriesAsync();
}
