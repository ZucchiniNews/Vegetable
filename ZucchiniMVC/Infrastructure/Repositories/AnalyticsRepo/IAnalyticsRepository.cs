using Zucchinimvc.Application.Services.Analytics.DTOs;

namespace Zucchinimvc.Infrastructure.Repositories.AnalyticsRepo;
public interface IAnalyticsRepository
{
    Task<(int Views, int UniqueVisitors)> GetSummaryAsync(DateTime from, DateTime to);
    Task<List<TopArticleDto>> GetTopArticleAsync(int n);
    Task<int> GetArticleViewCountAsync(string slug);
}
