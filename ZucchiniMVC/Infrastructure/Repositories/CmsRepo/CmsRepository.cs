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
                Cover = dto.Cover != null ? new ArticleCover
                {
                    OriginalUrl = dto.Cover.Url,
                    ThumbnailUrl = dto.Cover.Formats?.Thumbnail?.Url
                } : null
            }).ToList();
        }

        public async Task<Article> GetArticleBySlugAsync(string slug)
        {
            var encodedSlug = Uri.EscapeDataString(slug);
            var articleDtos = await _CmsClient.GetAsync<IEnumerable<ArticleDto>>($"articles?filters[slug][$eq]={encodedSlug}&populate=*");
            var articleDto = articleDtos.FirstOrDefault();
            if (articleDto == null)
                return null;
            return new Article
            {
                Id = articleDto.Id,
                Title = articleDto.Title,
                ContentSummary = articleDto.ContentSummary,
                Slug = articleDto.Slug,
                CreatedAt = articleDto.CreatedAt,
                UpdatedAt = articleDto.UpdatedAt,
                PublishedAt = articleDto.PublishedAt,
                BodyPreview = articleDto.BodyPreview,
                BodyGated = articleDto.BodyGated,
                EditorsChoice = articleDto.EditorsChoice,
                Cover = articleDto.Cover != null ? new ArticleCover
                {
                    OriginalUrl = articleDto.Cover.Url,
                    ThumbnailUrl = articleDto.Cover.Formats?.Thumbnail?.Url
                } : null
            };
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
            });
        }


        public async Task<IEnumerable<Article>> GetArticlesByCategoryAsync(string categorySlug)
        {

            var encodedCategory = Uri.EscapeDataString(categorySlug);
            var articleDtos = await _CmsClient.GetAsync<IEnumerable<ArticleDto>>($"articles?filters[category][slug][$eq]={encodedCategory}&populate=*");
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
                Cover = dto.Cover != null ? new ArticleCover
                {
                    OriginalUrl = dto.Cover.Url,
                    ThumbnailUrl = dto.Cover.Formats?.Thumbnail?.Url
                } : null
            }).ToList();
        }
    }
}

