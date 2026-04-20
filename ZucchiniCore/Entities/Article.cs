namespace ZucchiniCore.Entities;

public class Article
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime PublishedAt { get; set; }
    public ArticleCover? Cover { get; set; }
}

public class ArticleCover
{
    public string Url { get; set; } = string.Empty;
}