using Recipe_API.DTO.Category;
using Recipe_API.Entities;
using Recipe_API.Services;
using RecipeAPI.Data;
using System.Linq;

namespace RecipeAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public Category CreateCategory(CategoryCreateDto categoryCreateDto)
        {
            var newCategory = new Category { Name = categoryCreateDto.Name };
            _context.Categories.Add(newCategory);
            _context.SaveChanges();
            return newCategory;
        }
    }
}
