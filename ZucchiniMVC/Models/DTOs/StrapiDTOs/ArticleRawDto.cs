public class ArticleRawDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public CoverRawDto? Cover { get; set; } 
}

public class CoverRawDto
{
    public string Url { get; set; } = string.Empty;
}