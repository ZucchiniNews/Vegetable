namespace ZucchiniCore.Entities;

public class UserLikedArticle
{
    public string UserId { get; set; } = string.Empty;
    public int ArticleId { get; set; }
    public User User { get; set; } = null!;

}


