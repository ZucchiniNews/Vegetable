namespace Infrastrcture.ApiClients.StrapiClient;

public interface IStrapiClient
{
    Task<StrapiResponse<ArticleDto>> GetArticlesAsync(string endpoint);
}