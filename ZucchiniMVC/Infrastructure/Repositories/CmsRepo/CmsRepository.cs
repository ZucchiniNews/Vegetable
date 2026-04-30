using ZucchiniCore.Entities;
using Zucchinimvc.Models.DTOs.StrapiDTOs;


namespace Zucchinimvc.Infrastructure.Repositories.CmsRepo
{
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
                ContentSummary = dto.ContentSummary,
                Slug = dto.Slug,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                PublishedAt = dto.PublishedAt,
                BodyPreview = dto.BodyPreview,
                BodyGated = dto.BodyGated,
                EditorsChoice = dto.EditorsChoice,
                Cover = dto.Cover != null ? new ArticleCover {
                    OriginalUrl = dto.Cover.Url,
                    ThumbnailUrl = dto.Cover.Formats?.Thumbnail?.Url } : null
            }).ToList();
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
                    ContentSummary = a.ContentSummary,
                    Slug = a.Slug,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    PublishedAt = a.PublishedAt,
                    BodyPreview = a.BodyPreview,
                    BodyGated = a.BodyGated,
                    EditorsChoice = a.EditorsChoice
                }).ToList()
            });
        }
    }
}

