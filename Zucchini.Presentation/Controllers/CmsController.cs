using Microsoft.AspNetCore.Mvc;
using Application.Services.CMS;

namespace Presentation.Controllers;

public class CmsController : Controller
{
    private readonly ICmsService _cmsService;

    public CmsController(ICmsService cmsService)
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
}
