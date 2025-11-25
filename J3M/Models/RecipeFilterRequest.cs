namespace J3M.Models
{
    internal class RecipeFilterRequest
    {
        public List<int> AllergyIds { get; set; }
        public List<int> DietIds { get; set; }
    }
}