using Microsoft.AspNetCore.Mvc;
using Zucchinimvc.Application.Services.Searches;
using Zucchinimvc.Application.Services.Searches.DTOs;

namespace Zucchinimvc.ViewComponents
{
    public class SearchViewComponent : ViewComponent
    {
        private readonly ISearchService _searchService;

        public SearchViewComponent(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? query)
        {
            var results = string.IsNullOrWhiteSpace(query)
                ? Enumerable.Empty<ArticlesSearchResultDTO>()
                : await _searchService.GetSearchResult(query);

            return View(results);
        }
    }
}
