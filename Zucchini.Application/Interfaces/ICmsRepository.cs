using Domain.Entities;

namespace Application.Interfaces;

public interface ICmsRepository
{
    Task<IEnumerable<Article>> GetArticlesAsync();
    Task<IEnumerable<Category>> GetCategoriesAsync();
}
