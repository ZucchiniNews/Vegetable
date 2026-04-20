namespace ZucchiniCore.Entities;

public class ArticleCover
{
    public string OriginalUrl { get; set; } = string.Empty;  // from CMS
    public string? BlobUrl { get; set; }                     // set after blob processing
    public string? ThumbnailUrl { get; set; }                // set after resize trigger fires


    public string DisplayUrl => ThumbnailUrl ?? OriginalUrl;
}
