using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using ZucchiniCore.Entities;
using Zucchinimvc.Infrastructure.Data;

namespace Zucchinimvc.Application.Services.Articles;

public class UtilsService : IUtilsService
{
    private readonly ApplicationDbContext _context;
    private const int WordsPerMinute = 225;

    public UtilsService(ApplicationDbContext context)
    {
        _context = context;
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
        return await _context.UserLikedArticles
            .CountAsync(ul => ul.ArticleId == articleId);
    }

    public async Task<bool> IsLikedByUserAsync(int articleId, string userId)
    {
        if (string.IsNullOrEmpty(userId)) return false;

        return await _context.UserLikedArticles
            .AnyAsync(ul => ul.ArticleId == articleId && ul.UserId == userId);
    }

    public async Task ToggleLikeAsync(int articleId, string userId)
    {
        var existingLike = await _context.UserLikedArticles
            .FirstOrDefaultAsync(ul => ul.ArticleId == articleId && ul.UserId == userId);

        if (existingLike != null)
        {
            _context.UserLikedArticles.Remove(existingLike);
        }
        else
        {
            var like = new UserLikedArticle
            {
                ArticleId = articleId,
                UserId = userId
            };

        Console.WriteLine($"Saving like: ArticleId={articleId}, UserId={userId}");
            _context.UserLikedArticles.Add(like);
        }

        await _context.SaveChangesAsync();
        
    }
}
