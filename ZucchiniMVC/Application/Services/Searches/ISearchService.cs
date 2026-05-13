using Zucchinimvc.Models.DTOs.SearchDTOs;

namespace Zucchinimvc.Application.Services.Searches
{
    public interface ISearchService
    {
        Task<IEnumerable<ArticlesSearchResultDTO>> GetSearchResult(string query);
    }
}