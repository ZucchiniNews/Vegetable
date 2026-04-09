using NuGet.Protocol.Core.Types;
using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.Repositories.CmsRepo;
using Zucchinimvc.Models.DTOs.StrapiDTOs;

namespace Zucchinimvc.Application.Services.CMS;

public class CmsService : ICmsService
{
    private readonly ICmsRepository _cmsRepository;

    public CmsService(ICmsRepository cmsRepository)
    {
        _cmsRepository = cmsRepository;
    }

    public async Task<IEnumerable<Article>> GetArticles()
    {
        var articles = await _cmsRepository.GetArticlesAsync();

        return articles;
    }

    public async Task<List<Category>> GetCategories()
    {
        var categories = await _cmsRepository.GetCategoriesAsync();
        return categories.ToList();
    }
}