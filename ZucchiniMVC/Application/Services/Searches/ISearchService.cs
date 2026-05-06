namespace Zucchinimvc.Infrastructure.Services
{
    public interface ISearchService
    {
        Task<string> SearchArticlesByTitleAsync(string searchTerm);
    }
}