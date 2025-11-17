namespace J3M.Models
{
    public class RecipeSummaryCard
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; } = "";
        public int IngredientCount { get; set; }
        public int DietCount { get; set; }
        public int PrepTimeMinutes { get; set; }
    }
}
