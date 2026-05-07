using Azure.Monitor.Query;
using Azure.Core;
using ZucchiniCore.Entities;
using Zucchinimvc.Models.DTOs.Analytic;
using Zucchinimvc.Infrastructure.ApiClients.AzureInsightClient;

namespace Zucchinimvc.Application.Services.Analytics;

public class AnalyticsService : IAnalyticsService
{
    private readonly IAzureInsightClient _InsightClient;
    private readonly LogsQueryClient _logsQueryClient;
    private readonly string _workspaceId;
    public AnalyticsService(IAzureInsightClient InsightClient, LogsQueryClient logsQueryClient, IConfiguration configuration)
    {
        _InsightClient = InsightClient;
        _logsQueryClient = logsQueryClient;
        _workspaceId = configuration["ApplicationInsights:WorkspaceId"]!;
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
    
        var topArticlesQuery = $@"
            customEvents
            | whre timestamp >= ago(30d)
            | where name == 'ArticleView'
            | summarize ViewsCount = count() by ResourceId = tostring(customDimensions['ResourceId'])
            | order by ViewCount desc
            | take 5";

        var topArticlesResponse = await _logsQueryClient.QueryResourceAsync(
            new ResourceIdentifier("/subscriptions/f6c79cfc-dcff-4cec-8cc0-8cbbd35495fa/resourceGroups/Gr25-17RG/providers/microsoft.insights/components/ZucchiniNews"),
            topArticlesQuery,
            QueryTimeRange.All
        );
        
        var response = await _logsQueryClient.QueryResourceAsync(
            new ResourceIdentifier("/subscriptions/f6c79cfc-dcff-4cec-8cc0-8cbbd35495fa/resourceGroups/Gr25-17RG/providers/microsoft.insights/components/ZucchiniNews"),
            query,
            QueryTimeRange.All
        );

        var table = response.Value.Table;
        var row = table.Rows.FirstOrDefault();
        
        return new AnalyticsSummaryDto
        {
            Views = row != null ? (int)(long)row["TotalRequests"] : 0,
            UniqueVisitors = row != null ? (int)(long)row["UniqueUsers"] : 0,
        };
    }
}
