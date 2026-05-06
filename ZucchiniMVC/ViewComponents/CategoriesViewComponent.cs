
using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Models.ViewModels;
namespace Zucchinimvc.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ICmsService _cmsService;

        public CategoriesViewComponent(ICmsService cmsService)
        {
            _cmsService = cmsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _cmsService.GetAllCategories();

            var maxVisible = 4;

            var model = new CategoriesViewModel
            {
                VisibleCategories = categories.Take(maxVisible).ToList(),
                ExtraCategories = categories.Skip(maxVisible).ToList(),
                CurrentSlug = RouteData.Values["slug"]?.ToString()
            };

            return View(model);
        }
    }

}

