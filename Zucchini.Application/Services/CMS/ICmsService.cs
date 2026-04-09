using Domain.Entities;

namespace Application.Services.CMS;

public interface ICmsService
{
    Task<List<Article>> GetArticles();
    Task<List<Category>> GetCategories();
}