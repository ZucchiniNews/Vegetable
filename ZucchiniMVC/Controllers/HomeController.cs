using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Zucchinimvc.Application.Services.Articles;
using Zucchinimvc.Application.Services.Analytics;
using Zucchinimvc.Application.Services.Subscriptions;
using Zucchinimvc.Models.ViewModels;
using ZucchiniCore.Entities;

namespace Zucchinimvc.Controllers;

public class HomeController : Controller
{
    private readonly ICmsService _cmsService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IUtilsService _utilsService;
    private readonly IAnalyticsService _analyticsService;

    public HomeController(ICmsService cmsService, ISubscriptionService subscriptionService, IUtilsService utilsService, IAnalyticsService analyticsService)
    {
        _cmsService = cmsService;
        _subscriptionService = subscriptionService;
        _utilsService = utilsService;
        _analyticsService = analyticsService;
    }

    public async Task<IActionResult> Index()
    {
        var editorsChoice = await _cmsService.GetEditorsChoice();
        var featured = await _cmsService.GetFeaturedArticle();
        var latest = await _cmsService.GetLatestArticles();

        return View(new HomeIndexViewModel
        {
            FeaturedArticle = featured == null ? null : new ArticleCardViewModel
            {
                Article = featured,
                ReadTimeMin = _utilsService.CalculateReadTime(featured.BodyPreview + featured.BodyGated)
            },
            EditorsChoiceArticles = editorsChoice.Select(a => new ArticleCardViewModel
            {
                Article = a,
                ReadTimeMin = _utilsService.CalculateReadTime(a.BodyPreview + a.BodyGated)
            }).ToList(),
            LatestArticles = latest.Select(a => new ArticleCardViewModel
            {
                Article = a,
                ReadTimeMin = _utilsService.CalculateReadTime(a.BodyPreview + a.BodyGated)
            }).ToList()
        });
    }

    [HttpGet("/article/{slug}")]
    public async Task<IActionResult> Article(string slug)
    {
        var article = await _cmsService.GetArticleBySlug(slug);

        if (article == null)
            return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        bool isActiveSubscription = false;
        var likeCount = await _utilsService.GetLikeCountAsync(article.Id);
        var isLiked = await _utilsService.IsLikedByUserAsync(article.Id, userId);
        var viewCount = await _analyticsService.GetArticleViewCountAsync(article.Slug);
        int readTime = _utilsService.CalculateReadTime(article.ContentSummary + article.BodyPreview + article.BodyGated);

        if (!string.IsNullOrEmpty(userId))
        {
            isActiveSubscription = await _subscriptionService.UserHasActiveSubscription(userId);
        }

        await _analyticsService.TrackAsync(EventType.ArticleView, slug, userId);

        return View(new ArticleViewModel
        {
            Article = article,
            LikeCount = likeCount,
            IsLikedByCurrentUser = isLiked,
            IsSubscribed = isActiveSubscription,
            Category = article.Category ?? throw new InvalidOperationException($"Article '{slug}' has no category assigned."),
            ViewCount = viewCount,
            ReadTimeMin = readTime
        });
    }

    [HttpGet("/category/{slug}")]
    public async Task<IActionResult> Category(string slug)
    {
        var articles = await _cmsService.GetArticlesByCategory(slug);
        return View("Category", articles);
    }

    public class LikeRequest { public int ArticleId { get; set; } }

    [HttpPost("/article/toggle-like")]
    [Authorize]
    public async Task<IActionResult> ToggleLike([FromBody] LikeRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        try
        {
            await _utilsService.ToggleLikeAsync(request.ArticleId, userId);
            var likeCount = await _utilsService.GetLikeCountAsync(request.ArticleId);
            return Json(new { success = true, likeCount });
        }
        catch (ArgumentException)
        {
            return BadRequest(new { error = "Invalid like request." });
        }
        catch (InvalidOperationException)
        {
            return BadRequest(new { error = "Unable to process like request." });
        }
    }

    [HttpGet("/Home/SearchSuggestions")]
    [AllowAnonymous]
    public IActionResult SearchSuggestions(string? query)
    {
        return ViewComponent("Search", new { query });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
