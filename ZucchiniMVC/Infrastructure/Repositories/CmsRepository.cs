using ZucchiniCore.Entities;

namespace Zucchinimvc.Infrastrcture.Repositories
{
    public class CmsRepository : ICmsRepository
    {
        private readonly StrapiClient _strapiClient;

        public CmsRepository(StrapiClient strapiClient)
        {
            _strapiClient = strapiClient;
        }

        public async Task<IEnumerable<Article>> GetArticlesAsync()
        {
            var articleDtos = await _strapiClient.GetArticlesAsync<ArticleDto>("articles");

            return articleDtos.Select(dto => new Article
            {
                Id = dto.Id,

            });
        }

    }
}

