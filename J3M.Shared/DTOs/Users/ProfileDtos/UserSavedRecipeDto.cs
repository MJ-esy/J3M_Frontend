using J3M.Shared.DTOs.Recipes;

namespace J3M.Shared.DTOs.Users.ProfileDtos
{
    public class UserSavedRecipeDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }

        public List<RecipeDetailDto> SavedRecipes { get; set; }
    }
}
