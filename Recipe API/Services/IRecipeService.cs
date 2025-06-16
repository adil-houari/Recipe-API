using Recipe_API.DTO.RecipesDto;
using Recipe_API.Entities;
using Recipe_API.Models;
using System.Collections.Generic;

namespace Recipe_API.Services
{
    public interface IRecipeService
    {
        IEnumerable<RecipeBasicDto> GetAllRecipes();
        RecipeDetailDto GetRecipeById(int id);
        Recipe CreateRecipe(RecipeCreateDto recipeCreateDto);
        IEnumerable<RecipeBasicDto> SearchRecipes(RecipeSearchOptions options);

        bool DeleteRecipe(int id);
    }
}

