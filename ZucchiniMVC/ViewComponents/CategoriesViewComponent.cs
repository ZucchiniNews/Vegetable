
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

            var preferredOrder = new List<string>
            {
                "Local",
                "Sweden",
                "World",
                "Politics",
                "Economy",
                "Technology",
                "Sport"
            };

            categories = categories
                .OrderBy(c =>
                {
                    var index = preferredOrder.IndexOf(c.Name);

                    return index == -1 ? int.MaxValue : index;  // Unknown categories go to the end
                })
                .ThenBy(c => c.Name) // optional: sort new/unknown ones alphabetically
                .ToList();

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

