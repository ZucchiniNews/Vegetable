using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Zucchinimvc.Models.DTOs.StrapiDTOs;
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

        Console.WriteLine($"Articles count: {articles.Count()}");

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
