using Microsoft.EntityFrameworkCore;
using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.Repositories.CmsRepo;
using Microsoft.Extensions.Caching.Memory;

namespace Zucchinimvc.Application.Services.CMS;

public class CmsService : ICmsService
{

    private readonly ICmsRepository _cmsRepository;
    private readonly IMemoryCache _cache;
    public CmsService(ICmsRepository cmsRepository, IMemoryCache cache)
    {
        _cmsRepository = cmsRepository;
        _cache = cache;
    }

    public async Task<IEnumerable<Article>> GetArticles()
    {
        return await _cache.GetOrCreateAsync(
            "cms-articles",
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromMinutes(15);

                return await _cmsRepository.GetArticlesAsync();
            }) ?? Enumerable.Empty<Article>();
    }

    public async Task<Article?> GetArticleBySlug(string slug)
    {
        var article = await _cmsRepository.GetArticleBySlugAsync(slug);
        return article;
    }

    public async Task<List<Category>> GetAllCategories()
    {
        return await _cache.GetOrCreateAsync(
        "cms-categories",
        async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow =
                TimeSpan.FromHours(1);

            var categories =
                await _cmsRepository.GetCategoriesAsync();

            return categories.ToList();
        }) ?? new List<Category>();
    }

    public async Task<List<Article>> GetArticlesByCategory(string categorySlug)
    {
        var articles = await _cmsRepository.GetArticlesByCategoryAsync(categorySlug);
        return articles.ToList();
    }

    public async Task<Article> GetFeaturedArticle()
    {
        var articles = await GetArticles();
        Article? featured = null;

        try
        {
            var topLikedArticleId = await _cmsRepository.GetUserLikedArticles()
                                      .GroupBy(ul => ul.ArticleId)
                                      .OrderByDescending(g => g.Count())
                                      .Select(g => g.Key)
                                      .FirstOrDefaultAsync();

            if (topLikedArticleId != 0)
                featured = articles.FirstOrDefault(a => a.Id == topLikedArticleId);
        }
        catch (DbUpdateException)
        {

        }

        return featured
            ?? articles.FirstOrDefault(a => a.EditorsChoice)
            ?? articles.OrderByDescending(a => a.PublishedAt).First();
    }

    public async Task<List<Article>> GetEditorsChoice()
    {
        var articles = await GetArticles();
        return articles.Where(a => a.EditorsChoice).ToList();
    }

    public async Task<List<Article>> GetLatestArticles()
    {
        int take = 6;
        var articles = await GetArticles();
        return articles
            .OrderByDescending(a => a.PublishedAt)
            .Take(take)
            .ToList();
    }
}