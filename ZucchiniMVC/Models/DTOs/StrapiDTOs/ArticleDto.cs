public class ArticleDto
{
    public int Id { get; set; }
    public string DocumentId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime PublishedAt { get; set; }
    public CoverDto? Cover { get; set; }
}

public class CoverDto
{
    public string Url { get; set; } = string.Empty;
}