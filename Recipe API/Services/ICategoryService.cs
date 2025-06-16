using Recipe_API.DTO.Category;
using Recipe_API.Entities;

namespace Recipe_API.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(int id);
        Category CreateCategory(CategoryCreateDto categoryCreateDto);
    }
}
