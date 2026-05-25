using ZucchiniCore.Entities;

namespace ZucchiniMVC.Application.Services.Recommendation;

public interface IRecommendationService
{
    Task<List<Article>> GetRecommendArticles(string userId);
}
