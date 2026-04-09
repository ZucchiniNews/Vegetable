using Domain.Entities;
using Application.Interfaces;
using Infrastructure.ApiClients.CmsClient;

namespace Infrastructure.Repositories;

public class CmsRepository : ICmsRepository
{
    private readonly CmsClient _CmsClient;

    public CmsRepository(CmsClient CmsClient)
    {
        _CmsClient = CmsClient;
    }

    public async Task<IEnumerable<Article>> GetArticlesAsync()
    {
        var articleDtos = await _CmsClient.GetAsync<IEnumerable<ArticleDto>>("articles?populate=*");

        return articleDtos.Select(dto => new Article
        {
            Id = dto.Id,
            Title = dto.Title,
            Description = dto.Description,
            Slug = dto.Slug,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            PublishedAt = dto.PublishedAt,
            Cover = dto.Cover != null ? new ArticleCover { Url = dto.Cover.Url } : null
        });
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        var categoryDtos = await _CmsClient.GetAsync<IEnumerable<CategoryDto>>("categories?populate=*");

        return categoryDtos.Select(dto => new Category
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Slug = dto.Slug,
            Articles = dto.Articles?.Select(a => new Article
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Slug = a.Slug,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt,
                PublishedAt = a.PublishedAt
            }).ToList()
        });
    }
}
