namespace Zucchinimvc.Models.DTOs.SearchDTOs
{
    public class ArticlesSearchResultDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }
}
