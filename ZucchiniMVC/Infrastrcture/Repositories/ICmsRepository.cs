using ZucchiniCore.Entities;
namespace Zucchinimvc.Infrastrcture.Repositories
{
    public interface ICmsRepository
    {
        Task<IEnumerable<Article>> GetArticlesAsync();
    }
}
