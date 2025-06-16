using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipe_API.DTO.RecipesDto;
using Recipe_API.Models;
using Recipe_API.Services;

namespace Recipe_API.Controllers
{
    [Route("api/recipe")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipesController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        // GET: api/recipe
        [HttpGet]
        public IActionResult GetAllRecipes()
        {
            var recipes = _recipeService.GetAllRecipes();
            return Ok(recipes);
        }

        // GET: api/recipe/{id}
        [HttpGet("{id}")]
        public IActionResult GetRecipeById(int id)
        {
            var recipe = _recipeService.GetRecipeById(id);
            if (recipe == null)
            {
                return NotFound();
            }
            return Ok(recipe);
        }

        // POST: api/recipe
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult CreateRecipe([FromBody] RecipeCreateDto recipeCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newRecipe = _recipeService.CreateRecipe(recipeCreateDto);
            return CreatedAtAction(nameof(GetRecipeById), new { id = newRecipe.Id }, newRecipe);
        }

        // DELETE: api/recipe/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult DeleteRecipe(int id)
        {
            var success = _recipeService.DeleteRecipe(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }


        [HttpGet("search")]
        public ActionResult<IEnumerable<RecipeBasicDto>> Search([FromQuery] RecipeSearchOptions options)
        {
            var recipes = _recipeService.SearchRecipes(options);
            return Ok(recipes);
        }

    }
}
