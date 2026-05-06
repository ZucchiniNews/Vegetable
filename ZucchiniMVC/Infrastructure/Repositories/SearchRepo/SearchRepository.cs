using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Zucchinimvc.Infrastructure.ApiClients.SearchClient;
using Zucchinimvc.Infrastructure.Config;

namespace Zucchinimvc.Infrastructure.Repositories.SearchRepo
{
    public class SearchRepository : ISearchRepository
    {
        private readonly SearchClient _searchClient;  

        public SearchRepository(SearchClient searchClient)
        {
            _searchClient = searchClient;
        }

        public async Task<string> SearchArticlesByTitleAsync(string searchTerm)
        {
            return await _searchClient.SearchAsync(searchTerm);
        }
    }
}
