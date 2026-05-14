using Azure;
using Azure.Search.Documents;
using Microsoft.Extensions.Options;
using Zucchinimvc.Infrastructure.Config;

namespace Zucchinimvc.Infrastructure.ApiClients.ZucchininSearchClient
{
    public class ZucchiniSearchClient
    {
        private readonly SearchSettings _settings;

        public SearchClient Client { get; }

        public ZucchiniSearchClient(IOptions<SearchSettings> settings)
        {
            _settings = settings.Value;

            Client = new SearchClient(
                new Uri(_settings.BaseUrl),
                _settings.IndexName,
                new AzureKeyCredential(_settings.ApiKey)
            );
        }
    }
}