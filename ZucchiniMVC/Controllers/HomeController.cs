using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.Subscriptions;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.Controllers;

public class HomeController : Controller
{
    private readonly ICmsService _cmsService;
    private readonly UserManager<User> _userManager;

    public HomeController(ICmsService cmsService, UserManager<User> userManager, ISubscriptionService subscriptionService)
    {
        _cmsService = cmsService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var articles = await _cmsService.GetArticles();
        ViewData["IsHome"] = true;
        return View(articles);
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

    [HttpGet("/article/{slug}")]
    public async Task<IActionResult> Article(string slug)
    {
        var articles = await _cmsService.GetArticles();
        var article = articles.FirstOrDefault(a => string.Equals(a.Slug, slug, StringComparison.OrdinalIgnoreCase));

        if (article == null)
            return NotFound();

        var user = await _userManager.GetUserAsync(User);
        // var isSubscribed = user != null && await _subscriptionService.HasActiveSubscriptionAsync(user.Id);
        return View(new ArticleDetailViewModel
        {
            Article = article,
            // IsSubscribed = isSubscribed
        });
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


}
