using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Zucchinimvc.Models.ViewModels;

namespace Zucchinimvc.Controllers;

public class HomeController : Controller
{
    private readonly ICmsService _cmsService;

    public HomeController(ICmsService cmsService)
    {
        _cmsService = cmsService;
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
        return View(article);
    }

    [HttpGet("/category/{slug}")]
    public async Task<IActionResult> Category(string slug)
    {
        var articles = await _cmsService.GetArticlesByCategory(slug);
        return View("Index", articles);
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