namespace Zucchinimvc.Infrastructure.Services
{
    public interface ISearchService
    {
        Task<string> GetSearchResult(string searchTerm);
    }
}