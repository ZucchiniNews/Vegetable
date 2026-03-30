public interface IArticleService
{
    Task<List<ArticleDto>> GetArticles();
}