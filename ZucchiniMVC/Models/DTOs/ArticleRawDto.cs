public class ArticleRawDto
{
    public int Id { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }

    public CoverRawDto Cover { get; set; }
}

public class CoverRawDto
{
    public string Url { get; set; }
}