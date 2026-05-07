namespace Zucchinimvc.Infrastructure.Repositories.SearchRepo
{
    public interface ISearchRepository
    {
        Task<string> SearchGetResultAsync(string searchTerm);
    }
}
