using ZucchiniCore.Entities;
using Zucchinimvc.Models.DTOs.StrapiDTOs;

public interface ICmsService
{
    Task<IEnumerable<Article>> GetArticles();
    Task<List<Category>> GetCategories();
}