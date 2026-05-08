using Zucchinimvc.Models.DTOs.SearchDTOs;

namespace Zucchinimvc.Infrastructure.Repositories.SearchRepo
{
    public interface ISearchRepository
    {
        Task<IEnumerable<ArticlesSearchResultDTO>> SearchGetResultAsync(string query);
    }
}
