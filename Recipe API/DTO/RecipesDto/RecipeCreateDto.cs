using Recipe_API.Entities;

namespace Recipe_API.DTO.RecipesDto
{
    public class RecipeCreateDto
    {
        public string Title { get; set; }
        public int Time { get; set; }
        public Difficulty Difficulty { get; set; }
        public int CategoryId { get; set; }
        public List<IngredientCreateDto> Ingredients { get; set; }
    }
}
