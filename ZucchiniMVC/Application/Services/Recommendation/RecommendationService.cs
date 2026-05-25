using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.Repositories.CmsRepo;
using Microsoft.EntityFrameworkCore;

namespace ZucchiniMVC.Application.Services.Recommendation
{
    public class RecommendationService : IRecommendationService
    {
        private readonly ICmsRepository _cmsRepository;

        public RecommendationService(ICmsRepository cmsRepository)
        {
            _cmsRepository = cmsRepository;
        }
        public async Task<List<Article>> GetRecommendArticles(string userId)
        {
            var allArticles = (await _cmsRepository.GetArticlesAsync()).ToList();

            var likedArticleIds = await _cmsRepository.GetUserLikedArticles()
                .Where(ul => ul.UserId == userId)
                .Select(ul => ul.ArticleId)
                .ToListAsync();

            if (!likedArticleIds.Any())
            {
                return allArticles.OrderByDescending(a => a.PublishedAt).Take(6).ToList();
            }

            var topCategoryNames = allArticles
                .Where(a => likedArticleIds.Contains(a.Id) && a.Category != null)
                .GroupBy(a => a.Category!.Name)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(2)
                .ToList();

            return allArticles
                .Where(a => !likedArticleIds.Contains(a.Id))
                .Where(a => a.Category != null && topCategoryNames.Contains(a.Category.Name))
                .OrderByDescending(a => a.PublishedAt)
                .Take(4)
                .ToList();
        }
    }
}
