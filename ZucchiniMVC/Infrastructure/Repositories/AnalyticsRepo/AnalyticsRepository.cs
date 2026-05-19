using System.Configuration;
using Azure.Core;
using Azure.Monitor.Query;
using Zucchinimvc.Application.Services.Analytics.DTOs;
using Zucchinimvc.Infrastructure.ApiClients.LogQueryClient;

namespace Zucchinimvc.Infrastructure.Repositories.AnalyticsRepo;
public class AnalyticsRepository : IAnalyticsRepository
{
    private readonly LogsQueryClient _logQueryClient;
    private readonly string _resourceId;

    public AnalyticsRepository(ZuccLogQueryClient logQueryClient, IConfiguration configuration)
    {
        _logQueryClient = logQueryClient.GetClient();
        _resourceId = configuration["ApplicationInsights:ResourceId"]
            ?? throw new ArgumentNullException(
                "ApplicationInsights:ResourceId configuration is missing.");
    }

    public async Task<(int Views, int UniqueVisitors)> GetSummaryAsync(DateTime from, DateTime to)
    {
        var query = $@"
        requests
        | where timestamp >= ago(30d)
        | summarize TotalRequests = count(), UniqueUsers = dcount(user_Id)";

        var response = await _logQueryClient.QueryResourceAsync(
            new ResourceIdentifier(_resourceId),
            query,
            QueryTimeRange.All
        );

        var row = response.Value.Table.Rows.FirstOrDefault();

        return (
            Views: row != null ? (int)(long)row["TotalRequests"] : 0,
            UniqueVisitors: row != null ? (int)(long)row["UniqueUsers"] : 0
        );
    }

    public async Task<List<TopArticleDto>> GetTopArticleAsync(int n)
    {
        var query = $@"
        customEvents
        | where timestamp >= ago(30d)
        | where name == 'ArticleView'
        | summarize ViewCount = count() by ResourceId = tostring(customDimensions['ResourceId'])
        | order by ViewCount desc
        | take {n}";

        var response = await _logQueryClient.QueryResourceAsync(
            new ResourceIdentifier(_resourceId),
            query,
            QueryTimeRange.All
        );

        return response.Value.Table.Rows
        .Select(row => new TopArticleDto
        {
            ResourceId = (string)row["ResourceId"],
            ViewCount = (int)(long)row["ViewCount"]
        })
        .ToList();
    }

    public async Task<int> GetArticleViewCountAsync(string slug)
    {
        var query = $@"
        customEvents
        | where name == 'ArticleView'
        | where tostring(customDimensions['ResourceId']) == '{slug}'
        | summarize ViewCount = count()";

        var response = await _logQueryClient.QueryResourceAsync(
            new ResourceIdentifier(_resourceId),
            query,
            QueryTimeRange.All
        );

        var row = response.Value.Table.Rows.FirstOrDefault();
        return row != null ? (int)(long)row["ViewCount"] : 0;
    }
}