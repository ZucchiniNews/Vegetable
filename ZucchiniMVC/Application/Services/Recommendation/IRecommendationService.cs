using ZucchiniCore.Entities;

namespace ZucchiniMVC.Application.Services.RecommendationServcie;

public interface IRecommendationService
{
    Task<List<Article>> GetRecommendArticles(string userId);
}
