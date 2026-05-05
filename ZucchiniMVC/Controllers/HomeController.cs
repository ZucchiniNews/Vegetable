using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Zucchinimvc.Application.Services.Subscriptions;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.Controllers;

public class HomeController : Controller
{
    private readonly ICmsService _cmsService;
    private readonly ISubscriptionService _subscriptionService;

    public HomeController(ICmsService cmsService, ISubscriptionService subscriptionService)
    {
        _cmsService = cmsService;
        _subscriptionService = subscriptionService;
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
        var categories = await _cmsService.GetAllCategories();
        return View(categories);
    }

    [HttpGet("/category/{slug}")]
    public async Task<IActionResult> Category(string slug)
    {

        Console.WriteLine($"Fetching articles for category slug: {slug}");
        var articles = await _cmsService.GetArticlesByCategory(slug);

        return View("Index", articles);
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
