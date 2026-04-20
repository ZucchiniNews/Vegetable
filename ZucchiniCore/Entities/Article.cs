namespace ZucchiniCore.Entities;

public class Article
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ContentSummary { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string BodyPreview { get; set; } = string.Empty;
    public string BodyGated { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime PublishedAt { get; set; }
    public ArticleCover? Cover { get; set; }
    public int ReadingTimeMinutes { get; set; } = 1;
}

public class ArticleCover
{
    public string Url { get; set; } = string.Empty;
}