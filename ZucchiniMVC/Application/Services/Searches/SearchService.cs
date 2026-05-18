using Zucchinimvc.Application.Services.Searches.DTOs;
using Zucchinimvc.Infrastructure.Repositories.SearchRepo;

namespace Zucchinimvc.Application.Services.Searches
{
    public class SearchService : ISearchService
    {
        private readonly ISearchRepository _searchRepository;

        public SearchService(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        public async Task<IEnumerable<ArticlesSearchResultDTO>> GetSearchResult(string query)
        {

            if (string.IsNullOrWhiteSpace(query))
                return Enumerable.Empty<ArticlesSearchResultDTO>();

            return await _searchRepository.SearchGetResultAsync(query);
        }
    }
}