using Azure.Core;
using Azure.Monitor.Query;
using ZucchiniCore.enums;
using Zucchinimvc.Application.Services.Analytics.DTOs;
using Zucchinimvc.Infrastructure.ApiClients.AzureInsightClient;
using Zucchinimvc.Infrastructure.Repositories.AnalyticsRepo;

namespace Zucchinimvc.Application.Services.Analytics;

public class AnalyticsService : IAnalyticsService
{
    private readonly IAzureInsightClient _InsightClient;
    private readonly IAnalyticsRepository _analyticsRepository;
    public AnalyticsService(IAzureInsightClient InsightClient, IAnalyticsRepository analyticsRepository)
    {
        _InsightClient = InsightClient;
        _analyticsRepository = analyticsRepository;
    }

    public async Task TrackAsync(EventType eventType, string resourceId, string? userId = null)
    {
        var dto = new AnalyticsEventDto
        {
            EventType = eventType,
            ResourceId = resourceId,
            UserId = userId,
            Timestamp = DateTime.UtcNow
        };

        await _InsightClient.TrackEventAsync(dto);
    }

    public async Task<AnalyticsSummaryDto> GetDashboardSummaryAsync(DateTime from, DateTime to)
    {
        var (views, uniqueVistors) = await _analyticsRepository.GetSummaryAsync(from, to);
        var topArticles = await _analyticsRepository.GetTopArticleAsync(5);

        return new AnalyticsSummaryDto
        {
            Views = views,
            UniqueVisitors = uniqueVistors,
            TopArticles = topArticles
        };
    }

    public async Task<int> GetArticleViewCountAsync(string slug)
    {
        return await _analyticsRepository.GetArticleViewCountAsync(slug);
    }
}
