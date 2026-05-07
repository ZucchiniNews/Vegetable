using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Infrastructure.Services;

namespace Zucchinimvc.ViewComponents
{
    public class SearchViewComponent : ViewComponent
    {
        private readonly ISearchService _searchService;

        public SearchViewComponent(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string query)
        {
            var results = await _searchService.SearchArticlesByTitleAsync(query);
            return View(results);
        }
    }
}
