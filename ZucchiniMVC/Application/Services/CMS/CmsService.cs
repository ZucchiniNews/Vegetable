using NuGet.Protocol.Core.Types;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.Articles;
using Zucchinimvc.Infrastructure.Repositories.CmsRepo;
using Zucchinimvc.Models.DTOs.StrapiDTOs;

namespace Zucchinimvc.Application.Services.CMS;

public class CmsService : ICmsService
{
    private readonly ICmsRepository _cmsRepository;
    private readonly IArticleService _articleService;
    public CmsService(ICmsRepository cmsRepository, IArticleService articleService)
    {
        _cmsRepository = cmsRepository;
        _articleService = articleService;
    }

    public async Task<IEnumerable<Article>> GetArticles()
    {
        var articles = await _cmsRepository.GetArticlesAsync();

        foreach (var article in articles)
        {
            var totalContent = $"{article.BodyPreview} {article.BodyGated}";
            article.ReadingTimeMinutes = _articleService.CalculateReadTime(totalContent);
        }

        return articles;
    }

    public async Task<List<Category>> GetCategories()
    {
        var categories = await _cmsRepository.GetCategoriesAsync();
        return categories.ToList();
    }
}