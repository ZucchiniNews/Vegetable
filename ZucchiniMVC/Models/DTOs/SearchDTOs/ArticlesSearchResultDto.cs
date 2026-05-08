using System.Text.Json.Serialization;

namespace Zucchinimvc.Models.DTOs.SearchDTOs
{
    public class ArticlesSearchResultDTO
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("slug")]
        public string Slug { get; set; } = string.Empty;
    }
}
