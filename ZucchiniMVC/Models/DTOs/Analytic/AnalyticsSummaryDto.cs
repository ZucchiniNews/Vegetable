namespace Zucchinimvc.Models.DTOs.Analytic;

public class AnalyticsSummaryDto
{
    public int Views { get; set; }
    public int UniqueVisitors { get; set; }
    public int NewSubscriptions { get; set; }
    public int CanceledSubscriptions { get; set; }
    public List<TopArticleDto> TopArticles { get; set; } = new();
}

public class TopArticleDto
{
    public string ResourceId { get; set; } = string.Empty;
    public int ViewCount { get; set; }
}
