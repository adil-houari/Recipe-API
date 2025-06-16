using Recipe_API.Entities;

namespace Recipe_API.DTO.RecipesDto
{
    public class RecipeDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Time { get; set; }
        public string Category { get; set; }
        public Difficulty Difficulty { get; set; }
        public IEnumerable<IngredientDto> Ingredients { get; set; }
    }
}
