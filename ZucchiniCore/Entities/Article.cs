namespace ZucchiniCore.Entities;

public class Article
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ContentSummary { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string BodyPreview { get; set; } = string.Empty;
    public string BodyGated { get; set; } = string.Empty;
    public bool EditorsChoice { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime PublishedAt { get; set; }
    public ArticleCover? Cover { get; set; }
    public ArticleCover? Thumbnail { get; set; }
    public int ReadingTimeMinutes { get; set; } = 1;
    public Category? Category { get; set; }
}

public class ArticleCover
{
    public string OriginalUrl { get; set; } = string.Empty;  // from CMS
    public string? BlobUrl { get; set; }                     // set after blob processing
    public string? ThumbnailUrl { get; set; }                // set after resize trigger fires
    public string DisplayUrl => ThumbnailUrl ?? OriginalUrl;
}

