using Zucchinimvc.Models.DTOs.SearchDTOs;

namespace Zucchinimvc.Infrastructure.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<ArticlesSearchResultDTO>> GetSearchResult(string query);
    }
}