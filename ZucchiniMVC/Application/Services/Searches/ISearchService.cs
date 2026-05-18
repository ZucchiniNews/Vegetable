using Zucchinimvc.Application.Services.Searches.DTOs;

namespace Zucchinimvc.Application.Services.Searches
{
    public interface ISearchService
    {
        Task<IEnumerable<ArticlesSearchResultDTO>> GetSearchResult(string query);
    }
}