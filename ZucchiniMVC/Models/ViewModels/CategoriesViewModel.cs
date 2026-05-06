
using ZucchiniCore.Entities;

namespace Zucchinimvc.Models.ViewModels
{
    public class CategoriesViewModel
    {
        public List<Category> VisibleCategories { get; set; } = new();
        public List<Category> ExtraCategories { get; set; } = new();
        public string? CurrentSlug { get; set; }
    }

}