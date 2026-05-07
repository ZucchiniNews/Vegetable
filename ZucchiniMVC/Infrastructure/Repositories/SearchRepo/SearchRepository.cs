
using Zucchinimvc.Infrastructure.ApiClients.ZucchininSearchClient;
using Zucchinimvc.Models.DTOs.SearchDTOs;

namespace Zucchinimvc.Infrastructure.Repositories.SearchRepo
{
    public class SearchRepository : ISearchRepository
    {
        private readonly ZucchininSearchClient _zucchininSearchClient;

        public SearchRepository(ZucchininSearchClient zucchininSearchClient)
        {
            _zucchininSearchClient = zucchininSearchClient;
        }

        public async Task<IEnumerable<ArticlesSearchResultDTO>> SearchGetResultAsync(string query)
        {
            var response = await _zucchininSearchClient.Client.SearchAsync<ArticlesSearchResultDTO>(query);

            var results = new List<ArticlesSearchResultDTO>();
            await foreach (var result in response.Value.GetResultsAsync())
            {
                results.Add(result.Document);
            }
            return results;
        }
    }
}
