using J3M.Shared.DTOs.Recipes;
using J3M.Shared.DTOs.Users.ProfileDtos;

namespace J3M.Models
{
    public class UserProfileViewModel
    {
        public string AvatarUrl { get; set; } = "/images/avatar.png";

        // Backend user info
        public int UserId { get; set; }
        public string FullName { get; set; } = "";

        public string Email { get; set; } = "";

        // Links
        public string WeeklyMealPlannerUrl { get; set; } = "/MealPlanner/Index";

        // Saved recipes with summaries
        public List<RecipeDetailDto>? SavedRecipes { get; set; } = new();
        public UserProfileDto? Profile { get; set; }
    }
}
