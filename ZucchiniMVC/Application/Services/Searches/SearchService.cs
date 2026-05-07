
using Zucchinimvc.Infrastructure.Repositories.SearchRepo;

namespace Zucchinimvc.Infrastructure.Services
{
    public class SearchService : ISearchService
    {
        private readonly ISearchRepository _searchRepository;

        public SearchService(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        public async Task<string> GetSearchResult(string searchTerm)
        {
           
            if (string.IsNullOrWhiteSpace(searchTerm))
                return "[]";

            return await _searchRepository.SearchGetResultAsync(searchTerm);
        }
    }
}