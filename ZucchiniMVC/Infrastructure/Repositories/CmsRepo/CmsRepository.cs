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

        private static Article MapToArticle(ArticleDto dto) => new Article
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
            } : null,
            Category = dto.Category != null ? new Category
            {
                Id = dto.Category.Id,
                Name = dto.Category.Name,
                Slug = dto.Category.Slug,
                Description = dto.Category.Description
            } : null
        };
        public async Task<IEnumerable<Article>> GetArticlesAsync()
        {
            var articleDtos = await _CmsClient.GetAsync<IEnumerable<ArticleDto>>("articles?populate=*");

            return articleDtos.Select(MapToArticle).ToList();
        }
        public async Task<Article> GetArticleBySlugAsync(string slug)
        {
            var encodedSlug = Uri.EscapeDataString(slug);
            var articleDtos = await _CmsClient.GetAsync<IEnumerable<ArticleDto>>($"articles?filters[slug][$eq]={encodedSlug}&populate=*");
            var dto = articleDtos.FirstOrDefault();
            return dto == null ? null : MapToArticle(dto);
        }
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var categoryDtos = await _CmsClient.GetAsync<IEnumerable<CategoryDto>>("categories?populate=*");
            return categoryDtos.Select(dto => new Category
            {
                Id = dto.Id,
                Name = dto.Name,
                Slug = dto.Slug,
                Description = dto.Description
            });
        }
        public async Task<IEnumerable<Article>> GetArticlesByCategoryAsync(string categorySlug)
        {

            var encodedCategory = Uri.EscapeDataString(categorySlug);
            var articleDtos = await _CmsClient.GetAsync<IEnumerable<ArticleDto>>($"articles?filters[category][slug][$eq]={encodedCategory}&populate=*");
            return articleDtos.Select(MapToArticle).ToList();
        }
    }
}

