using Microsoft.AspNetCore.Mvc;

namespace Zucchinimvc.Controllers
{
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
    }
}
