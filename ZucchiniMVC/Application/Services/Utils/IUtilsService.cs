namespace Zucchinimvc.Application.Services.Articles;

public interface IUtilsService
{
    public int CalculateReadTime(string content);
    Task<int> GetLikeCountAsync(int articleId);
    Task<bool> IsLikedByUserAsync(int articleId, string userId);
    Task ToggleLikeAsync(int articleId, string userId);
}
