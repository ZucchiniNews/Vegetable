
using Azure.Search.Documents;
using Zucchinimvc.Infrastructure.ApiClients.ZucchininSearchClient;
using Zucchinimvc.Models.DTOs.SearchDTOs;

namespace Zucchinimvc.Infrastructure.Repositories.SearchRepo
{
    public class SearchRepository : ISearchRepository
    {
        private readonly ZucchiniSearchClient _zucchiniSearchClient;

        public SearchRepository(ZucchiniSearchClient zucchiniSearchClient)
        {
            _zucchiniSearchClient = zucchiniSearchClient;
        }

        public async Task<IEnumerable<ArticlesSearchResultDTO>> SearchGetResultAsync(string query)
        {
            var response = await _zucchiniSearchClient.Client
                .SearchAsync<ArticlesSearchResultDTO>(query, new SearchOptions
                {
                    Size = 5
                });

            var results = new List<ArticlesSearchResultDTO>();

            await foreach (var result in response.Value.GetResultsAsync())
            {
                results.Add(result.Document);
            }

            return results;
        }
    }
}
