using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.Repositories.CmsRepo;

namespace Zucchinimvc.Application.Services.CMS;

public class CmsService : ICmsService
{
    private readonly ICmsRepository _cmsRepository;

    public CmsService(ICmsRepository cmsRepository)
    {
        _cmsRepository = cmsRepository;
    }

    public async Task<List<Article>> GetArticles()
    {
        var articles = await _cmsRepository.GetArticlesAsync();
        return articles.ToList();
    }

    public async Task<List<Category>> GetCategories()
    {
        var categories = await _cmsRepository.GetCategoriesAsync();
        return categories.ToList();
    }
}