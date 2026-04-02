using ZucchiniCore.Entities;
using Zucchinimvc.Infrastrcture.Repositories;

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
}