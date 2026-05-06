namespace Zucchinimvc.Infrastructure.Repositories.SearchRepo
{
    public interface ISearchRepository
    {
        Task<string> SearchArticlesByTitleAsync(string searchTerm);
    }
}
