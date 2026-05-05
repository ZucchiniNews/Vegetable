using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.Articles;
using Zucchinimvc.Application.Services.Subscriptions;
using Zucchinimvc.Infrastructure.Repositories.CmsRepo;
using Zucchinimvc.Models.ViewModels;


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

        foreach (var article in articles)
        {
            var totalContent = $"{article.BodyPreview} {article.BodyGated}";
        }

        return articles;
    }

    public async Task<Article> GetArticleBySlug(string slug)
    {
        var article = await _cmsRepository.GetArticleBySlugAsync(slug);
        return article;
    }

    public async Task<List<Category>> GetCategories()
    {
        var categories = await _cmsRepository.GetCategoriesAsync();
        return categories.ToList();
    }
}