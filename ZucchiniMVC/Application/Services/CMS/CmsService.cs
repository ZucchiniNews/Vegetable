using Microsoft.EntityFrameworkCore;
using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.Data;
using Zucchinimvc.Infrastructure.Repositories.CmsRepo;

namespace Zucchinimvc.Application.Services.CMS;

public class CmsService : ICmsService
{
    private readonly ApplicationDbContext _context;
    private readonly ICmsRepository _cmsRepository;
    public CmsService(ICmsRepository cmsRepository, ApplicationDbContext context)
    {
        _cmsRepository = cmsRepository;
        _context = context;
    }

    public async Task<IEnumerable<Article>> GetArticles()
    {
        var articles = await _cmsRepository.GetArticlesAsync();

        foreach (var article in articles)
        {
            var totalContent = $"{article.BodyPreview} {article.BodyGated}";
        }

        return articles;
    }

    public async Task<Article?> GetArticleBySlug(string slug)
    {
        var article = await _cmsRepository.GetArticleBySlugAsync(slug);
        return article;
    }

    public async Task<List<Category>> GetAllCategories()
    {
        var categories = await _cmsRepository.GetCategoriesAsync();
        return categories.ToList();
    }

    public async Task<List<Article>> GetArticlesByCategory(string categorySlug)
    {
        var articles = await _cmsRepository.GetArticlesByCategoryAsync(categorySlug);
        return articles.ToList();
    }

    public async Task<Article> GetFeaturedArticle()
    {
        var articles = await _cmsRepository.GetArticlesAsync();
        Article? featured = null;

        try
        {
            var topLikedArticleId = await _context.UserLikedArticles
                .GroupBy(ul => ul.ArticleId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            if (topLikedArticleId != 0)
                featured = articles.FirstOrDefault(a => a.Id == topLikedArticleId);
        }
        catch
        {

        }

        return featured
            ?? articles.FirstOrDefault(a => a.EditorsChoice)
            ?? articles.OrderByDescending(a => a.PublishedAt).First();
    }

    public async Task<List<Article>> GetEditorsChoice()
    {
        var articles = await _cmsRepository.GetArticlesAsync();
        return articles.Where(a => a.EditorsChoice).ToList();
    }

    public async Task<List<Article>> GetLatest()
    {
        int take = 6;
        var articles = await _cmsRepository.GetArticlesAsync();
        return articles
            .OrderByDescending(a => a.PublishedAt)
            .Take(take)
            .ToList();
    }
}