using ZucchiniCore.Entities;
using Zucchinimvc.Models.DTOs.Analytic;

namespace Zucchinimvc.Application.Services.Analytics;

public interface IAnalyticsService
{
    Task TrackAsync(EventType eventType, string resourceId, string? userId = null);
    Task<AnalyticsSummaryDto> GetDashboardSummaryAsync(DateTime from, DateTime to);
    Task<int> GetArticleViewCountAsync(int articleId);
}
