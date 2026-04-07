using ZucchiniCore.Entities;

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
                DocumentId = dto.DocumentId,
                Title = dto.Title,
                Description = dto.Description,
                Slug = dto.Slug,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                PublishedAt = dto.PublishedAt,
                Cover = dto.Cover != null ? new ArticleCover { Url = dto.Cover.Url } : null
            });
        }
    }
}

