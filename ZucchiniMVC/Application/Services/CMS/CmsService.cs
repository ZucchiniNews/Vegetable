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
    private readonly IUtilsService _utilsService;
    private readonly ISubscriptionService _subscriptionService;
    public CmsService(ICmsRepository cmsRepository, IUtilsService utilsService, ISubscriptionService subscriptionService)
    {
        _cmsRepository = cmsRepository;
        _utilsService = utilsService;
        _subscriptionService = subscriptionService;
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


    public async Task<IActionResult> Details(string slug)
    {
        // 1. Get the article from CmsService
        var article = await GetArticleBySlug(slug);

        // 2. Get the current user ID
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // 3. Get Like data from UtilsService
        var likeCount = await _utilsService.GetLikeCountAsync(article.Id);
        var isLiked = await _utilsService.IsLikedByUserAsync(article.Id, userId);
        bool hasActiveSubscription = false;
        if (!string.IsNullOrEmpty(userId))
        {
            hasActiveSubscription = await _subscriptionService.UserHasActiveSubscription(userId);
        }

        article.ReadingTimeMinutes = _utilsService.CalculateReadTime($"{article.BodyPreview} {article.BodyGated}");

        var viewModel = new ArticleViewModel
        {
            Article = article,
            LikeCount = likeCount,
            IsLikedByCurrentUser = isLiked,
            IsSubscribed = hasActiveSubscription
        };

        return View(viewModel);
    }
    public async Task<List<Category>> GetCategories()
    {
        var categories = await _cmsRepository.GetCategoriesAsync();
        return categories.ToList();
    }
}