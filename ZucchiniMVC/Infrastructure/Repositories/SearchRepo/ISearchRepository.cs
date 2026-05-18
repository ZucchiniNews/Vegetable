using Zucchinimvc.Application.Services.Searches.DTOs;

namespace Zucchinimvc.Infrastructure.Repositories.SearchRepo
{
    public interface ISearchRepository
    {
        Task<IEnumerable<ArticlesSearchResultDTO>> SearchGetResultAsync(string query);
    }
}
