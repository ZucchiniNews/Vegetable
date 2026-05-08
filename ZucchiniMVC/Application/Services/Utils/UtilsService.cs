using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Zucchinimvc.Application.Services.Analytics;
using Zucchinimvc.Infrastructure.Data;
using Zucchinimvc.Infrastructure.Repositories.CmsRepo;

namespace Zucchinimvc.Application.Services.Articles;

public class UtilsService : IUtilsService
{
    private readonly ICmsRepository _cmsRepository;
    private readonly IAnalyticsService _analyticsService;
    private const int WordsPerMinute = 225;

    public UtilsService(ICmsRepository cmsRepository, IAnalyticsService analyticsService)
    {
        _cmsRepository = cmsRepository;
        _analyticsService = analyticsService;
    }
    public int CalculateReadTime(string content)
    {
        if (string.IsNullOrWhiteSpace(content)) return 0;

        var plainText = Regex.Replace(content, "<[^>]*>", "");

        var wordCount = plainText.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;

        return (int)Math.Ceiling((double)wordCount / WordsPerMinute);
    }

    public async Task<int> GetLikeCountAsync(int articleId)
    {
        return await _cmsRepository.GetLikeCountAsync(articleId);
    }

    public async Task<bool> IsLikedByUserAsync(int articleId, string userId)
    {
        if (string.IsNullOrEmpty(userId)) return false;

        return await _cmsRepository.IsLikedByUserAsync(articleId, userId);
    }
    public async Task ToggleLikeAsync(int articleId, string userId)
    {
        await _cmsRepository.ToggleLikeAsync(articleId, userId);
    }

    public async Task<int> GetViewCountAsync(string slug)
    {
        return await _analyticsService.GetArticleViewCountAsync(slug);
    }

}
