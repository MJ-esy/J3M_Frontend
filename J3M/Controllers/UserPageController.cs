using J3M.DTOs;
using J3M.Models;
using J3M.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;


namespace J3m_FE.Controllers
{
    public class UserPageController : Controller
    {
        public IActionResult Index()
        {
            // --- MOCK BACKEND DATA ---

            // Fake user from backend
            var mockUser = new
            {
                UserId = 42,
                FullName = "John Smith",
                Email = "Test@test.com"
            };

            // Fake recipe summaries (matching RecipeSummaryDto)
            var mockSavedRecipes = new List<RecipeSummaryCard>
            {
                new RecipeSummaryCard
                {
                    RecipeId = 1,
                    RecipeName = "Chicken Alfredo",
                    IngredientCount = 7,
                    DietCount = 1,
                    PrepTimeMinutes = 25
                },
                new RecipeSummaryCard
                {
                    RecipeId = 2,
                    RecipeName = "Greek Salad",
                    IngredientCount = 6,
                    DietCount = 2,
                    PrepTimeMinutes = 10
                }
            };

            // Build final view model
            var model = new UserProfileViewModel
            {
                UserId = mockUser.UserId,
                FullName = mockUser.FullName,
                Email = mockUser.Email, // backend saknar email
                AvatarUrl = "/images/placeholder-avatar.png",
                WeeklyMealPlannerUrl = "/MealPlanner/Index",
                SavedRecipes = mockSavedRecipes
            };

            return View(model);
        }
    }
}


    