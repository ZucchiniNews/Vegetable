namespace Zucchinimvc.Models.DTOs.StrapiDTOs;

public class ArticleDto
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
    public CoverDto? Cover { get; set; }
    public CoverFormatsDto? Thumbnail { get; set; }
    public CategoryDto? Category { get; set; }
}

public class CoverDto
{
    public string Url { get; set; } = string.Empty;
    public CoverFormatsDto? Formats { get; set; }
}

public class CoverFormatsDto
{
    public CoverFormatDto? Thumbnail { get; set; }
}

public class CoverFormatDto
{
    public string Url { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
}