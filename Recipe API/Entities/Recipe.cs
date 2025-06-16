namespace Recipe_API.Entities
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Time { get; set; }
        public Difficulty Difficulty { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
    }

    public enum Difficulty
    {
        Easy,
        Intermediate,
        Advanced
    }
}
