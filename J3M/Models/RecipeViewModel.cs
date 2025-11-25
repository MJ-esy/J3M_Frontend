

using J3M.Shared.DTOs.Recipes;

namespace J3M.Models
{
    public class RecipeViewModel
    {
        public List<RecipeDetailDto> Recipes { get; set; } = new();
        public List<AllergyOption> Allergies { get; set; } = new();
        public List<DietOption> Diets { get; set; } = new();
    }
}
