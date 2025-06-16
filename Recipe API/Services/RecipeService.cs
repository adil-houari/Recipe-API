using Recipe_API.DTO.RecipesDto;
using Recipe_API.Entities;
using Microsoft.EntityFrameworkCore;
using RecipeAPI.Data;
using Recipe_API.Models;

namespace Recipe_API.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly ApplicationDbContext _context;

        public RecipeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<RecipeBasicDto> GetAllRecipes()
        {
            return _context.Recipes
                .Include(r => r.Category) // Zorg ervoor dat je de Category entity includeert.
                .Select(r => new RecipeBasicDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    Time = r.Time,
                    Category = r.Category.Name,
                    Difficulty = r.Difficulty
                }).ToList();
        }

        public RecipeDetailDto? GetRecipeById(int id)
        {
            return _context.Recipes
                .Where(r => r.Id == id)
                .Include(r => r.Category)
                .Include(r => r.Ingredients)
                .Select(r => new RecipeDetailDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    Time = r.Time,
                    Category = r.Category.Name,
                    Difficulty = r.Difficulty,
                    Ingredients = r.Ingredients.Select(i => new IngredientDto
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Quantity = i.Quantity,
                        Unit = i.Unit
                    }).ToList()
                }).FirstOrDefault();
        }

        public Recipe CreateRecipe(RecipeCreateDto recipeCreateDto)
        {
            var newRecipe = new Recipe
            {
                Title = recipeCreateDto.Title,
                Time = recipeCreateDto.Time,
                Difficulty = recipeCreateDto.Difficulty,
                CategoryId = recipeCreateDto.CategoryId,
                Ingredients = recipeCreateDto.Ingredients.Select(i => new Ingredient
                {
                    Name = i.Name,
                    Quantity = i.Quantity,
                    Unit = i.Unit
                }).ToList()
            };

            _context.Recipes.Add(newRecipe);
            _context.SaveChanges();

            return newRecipe;
        }

        public bool DeleteRecipe(int id)
        {
            var recipe = _context.Recipes.Find(id);
            if (recipe == null)
            {
                return false;
            }

            _context.Recipes.Remove(recipe);
            _context.SaveChanges();
            return true;
        }


        //Zoekfunctionaliteit
        public IEnumerable<RecipeBasicDto> SearchRecipes(RecipeSearchOptions options)
        {
            var query = _context.Recipes.AsQueryable();

            if (!string.IsNullOrEmpty(options.SearchTerm))
            {
                query = query.Where(r => r.Title.Contains(options.SearchTerm));
            }
            if (options.MaxDifficulty.HasValue)
            {
                query = query.Where(r => (int)r.Difficulty <= options.MaxDifficulty.Value);
            }
            if (options.MaxTime.HasValue)
            {
                query = query.Where(r => r.Time <= options.MaxTime.Value);
            }
            if (options.Categories.Any())
            {
                query = query.Where(r => options.Categories.Contains(r.CategoryId));
            }

            return query.Select(r => new RecipeBasicDto
            {
                Id = r.Id,
                Title = r.Title,
                Time = r.Time,
                Category = r.Category.Name, 
                Difficulty = r.Difficulty
            }).ToList();
        }
    }
}
