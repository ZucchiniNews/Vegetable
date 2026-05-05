using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Zucchinimvc.Application.Services.Articles;
using Zucchinimvc.Application.Services.Subscriptions;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.Controllers;

public class HomeController : Controller
{
    private readonly ICmsService _cmsService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IUtilsService _utilsService;

    public HomeController(ICmsService cmsService, ISubscriptionService subscriptionService, IUtilsService utilsService)
    {
        _cmsService = cmsService;
        _subscriptionService = subscriptionService;
        _utilsService = utilsService;
    }

    public async Task<IActionResult> Index()
    {
        var articles = await _cmsService.GetArticles();
        return View(articles);
    }


    [HttpGet("/article/{slug}")]
    public async Task<IActionResult> Article(string slug)
    {
        var article = await _cmsService.GetArticleBySlug(slug);

        if (article == null)
            return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        bool isActiveSubscription = false;

        if (!string.IsNullOrEmpty(userId))
        {
            isActiveSubscription = await _subscriptionService.UserHasActiveSubscription(userId);
        }

        return View(new ArticleViewModel
        {
            Article = article,
            IsSubscribed = isActiveSubscription
        });
    }

    public async Task<IActionResult> Categories()
    {
        var categories = await _cmsService.GetCategories();
        return View(categories);
    }

    [HttpGet("/category/{slug}")]
    public async Task<IActionResult> Category(string slug)
    {
        var categories = await _cmsService.GetCategories();
        var category = categories.FirstOrDefault(c => string.Equals(c.Slug, slug, StringComparison.OrdinalIgnoreCase));
        if (category == null)
            return NotFound();
        var allArticles = await _cmsService.GetArticles();
        var categoryArticleIds = category.Articles?.Select(a => a.Id).ToHashSet() ?? new HashSet<int>();
        var categoryArticles = allArticles.Where(a => categoryArticleIds.Contains(a.Id)).ToList();
        return View("Index", categoryArticles);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ToggleLike(int articleId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _utilsService.ToggleLikeAsync(articleId, userId);

        var newCount = await _utilsService.GetLikeCountAsync(articleId);
        return Json(new { success = true, count = newCount });
    }
}
