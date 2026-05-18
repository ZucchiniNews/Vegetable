using Microsoft.EntityFrameworkCore;
using ZucchiniCore.Entities;
using Zucchinimvc.Application.Services.CMS.DTOs;
using Zucchinimvc.Infrastructure.Data;


namespace Zucchinimvc.Infrastructure.Repositories.CmsRepo
{
    public class CmsRepository : ICmsRepository
    {
        private readonly CmsClient _CmsClient;
        private readonly ApplicationDbContext _context;

        public CmsRepository(CmsClient CmsClient, ApplicationDbContext context)
        {
            _CmsClient = CmsClient;
            _context = context;
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

        public IQueryable<UserLikedArticle> GetUserLikedArticles()
        {
            return _context.UserLikedArticles;
        }

        public async Task<int> GetLikeCountAsync(int articleId)
        {
            return await _context.UserLikedArticles
                .CountAsync(ul => ul.ArticleId == articleId);
        }
        public async Task<bool> IsLikedByUserAsync(int articleId, string userId)
        {
            return await _context.UserLikedArticles
                .AnyAsync(ul => ul.ArticleId == articleId && ul.UserId == userId);
        }
        public async Task ToggleLikeAsync(int articleId, string userId)
        {
            var existingLike = await _context.UserLikedArticles
                .FirstOrDefaultAsync(ul => ul.ArticleId == articleId && ul.UserId == userId);

            if (existingLike != null)
                _context.UserLikedArticles.Remove(existingLike);
            else
                _context.UserLikedArticles.Add(new UserLikedArticle { ArticleId = articleId, UserId = userId });

            await _context.SaveChangesAsync();
        }
    }
}

