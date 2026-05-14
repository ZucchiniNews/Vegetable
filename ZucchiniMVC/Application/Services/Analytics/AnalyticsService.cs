using Azure.Core;
using Azure.Monitor.Query;
using ZucchiniCore.enums;
using Zucchinimvc.Infrastructure.ApiClients.AzureInsightClient;
using Zucchinimvc.Models.DTOs.Analytic;

namespace Zucchinimvc.Application.Services.Analytics;

public class AnalyticsService : IAnalyticsService
{
    private readonly IAzureInsightClient _InsightClient;
    private readonly LogsQueryClient _logsQueryClient;
    private readonly string _resourceId;
    public AnalyticsService(IAzureInsightClient InsightClient, LogsQueryClient logsQueryClient, IConfiguration configuration)
    {
        _InsightClient = InsightClient;
        _logsQueryClient = logsQueryClient;
        _resourceId = configuration["ApplicationInsights:ResourceId"]
            ?? throw new InvalidOperationException(
                "ApplicationInsights:ResourceId configuration is missing.");
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
        var query = $@"
            requests
            | where timestamp >= ago(30d)
            | summarize TotalRequests = count(), UniqueUsers = dcount(user_Id)
            // customEvents
            // | where timestamp between(datetime({from:O}) .. datetime({to:o}))";

        var response = await _logsQueryClient.QueryResourceAsync(
            new ResourceIdentifier(_resourceId),
            query,
            QueryTimeRange.All
        );

        var topArticlesQuery = $@"
            customEvents
            | where timestamp >= ago(30d)
            | where name == 'ArticleView'
            | summarize ViewCount = count() by ResourceId = tostring(customDimensions['ResourceId'])
            | order by ViewCount desc
            | take 5";

        var topArticlesResponse = await _logsQueryClient.QueryResourceAsync(
            new ResourceIdentifier(_resourceId),
            topArticlesQuery,
            QueryTimeRange.All
        );

        var table = response.Value.Table;
        var row = table.Rows.FirstOrDefault();
        var topArticles = topArticlesResponse.Value.Table.Rows
            .Select(row => new TopArticleDto
            {
                ResourceId = (string)row["ResourceId"],
                ViewCount = (int)(long)row["ViewCount"]
            })
        .ToList();

        return new AnalyticsSummaryDto
        {
            Views = row != null ? (int)(long)row["TotalRequests"] : 0,
            UniqueVisitors = row != null ? (int)(long)row["UniqueUsers"] : 0,
            TopArticles = topArticles
        };
    }

    public async Task<int> GetArticleViewCountAsync(string slug)
    {
        var query = $@"
        customEvents
        | where name == 'ArticleView'
        | where tostring(customDimensions['ResourceId']) == '{slug}'
        | summarize ViewCount = count()";

        var response = await _logsQueryClient.QueryResourceAsync(
            new ResourceIdentifier(_resourceId),
            query,
            QueryTimeRange.All
        );

        var row = response.Value.Table.Rows.FirstOrDefault();
        return row != null ? (int)(long)row["ViewCount"] : 0;
    }
}
