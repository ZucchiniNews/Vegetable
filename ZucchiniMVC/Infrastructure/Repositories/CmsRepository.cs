using ZucchiniCore.Entities;

namespace Zucchinimvc.Infrastrcture.Repositories
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
                // Map other properties
            });
        }
    }
}

