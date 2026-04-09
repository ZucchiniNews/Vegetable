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

    public async Task<IActionResult> Categories()
    {
        var categories = await _cmsService.GetCategories();
        return View(categories);
    }

    public IActionResult Local()
    {
        return View();
    }
    public IActionResult Sweden()
    {
        return View();
    }
    public IActionResult World()
    {
        return View();
    }
    public IActionResult Sport()
    {
        return View();
    }
    public IActionResult Economey()
    {
        return View();
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
