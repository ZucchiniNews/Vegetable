using ZucchiniCore.enums;
using Zucchinimvc.Application.Services.Analytics.DTOs;

namespace Zucchinimvc.Application.Services.Analytics;

public interface IAnalyticsService
{
    Task TrackAsync(EventType eventType, string resourceId, string? userId = null);
    Task<AnalyticsSummaryDto> GetDashboardSummaryAsync(DateTime from, DateTime to);
    Task<int> GetArticleViewCountAsync(string slug);
}
