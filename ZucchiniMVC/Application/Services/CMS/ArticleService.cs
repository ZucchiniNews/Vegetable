using Zucchinimvc.Infrastructure.ApiClients.StrapiClient;

namespace Zucchinimvc.Application.Services.CMS;

public class ArticleService : IArticleService
{
    private readonly IStrapiClient _strapiClient;

    public ArticleService(IStrapiClient strapiClient)
    {
        _strapiClient = strapiClient;
    }

    public async Task<List<ArticleDto>> GetArticles()
    {
        var result = await _strapiClient
            .GetAsync<ArticleRawDto>(
                "/api/articles?populate=cover"
            );

        return result.Data.Select(a => new ArticleDto
        {
            Id = a.Id,
            Title = a.Title,
            Description = a.Description,
            Slug = a.Slug,
            ImageUrl = a.Cover?.Url
        }).ToList();
    }
}