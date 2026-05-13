namespace Zucchinimvc.Application.Services.Utils;

public interface IUtilsService
{
    public int CalculateReadTime(string content);
    Task<int> GetLikeCountAsync(int articleId);
    Task<bool> IsLikedByUserAsync(int articleId, string userId);
    Task ToggleLikeAsync(int articleId, string userId);
    Task<int> GetViewCountAsync(string slug);
}
