namespace Zucchinimvc.Infrastructure.ApiClients.StrapiClient;

public interface IStrapiClient
{
    Task<IEnumerable<ArticleDto>> GetArticlesAsync<T>(string endpoint);
}