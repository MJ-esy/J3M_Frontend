using J3M.DTOs;

namespace J3M.Models.ViewModels
{
    // ViewModel for User Profile Page

    public class UserProfileViewModel
        {
            public string AvatarUrl { get; set; } = "/images/placeholder-avatar.png";

            // Backend user info
            public int UserId { get; set; }
            public string FullName { get; set; } = "";

            public string Email { get; set; } = "";

            // Links
            public string WeeklyMealPlannerUrl { get; set; } = "/MealPlanner/Index";

            // Saved recipes with summaries
            public List<RecipeSummaryCard> SavedRecipes { get; set; } = new();
        }

      
    

}
