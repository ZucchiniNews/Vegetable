using System;
using System.Collections.Generic;
using System.Text;

namespace ZucchiniCore.Entities;

public class UserLikedArticle
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = null!;
    public int ArticleId { get; set; }
}


