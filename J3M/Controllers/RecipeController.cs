using J3M.Services.Http;
using J3M.Models;
using J3M.Shared.DTOs.Allergies;
using J3M.Shared.DTOs.Diets;
using J3M.Shared.DTOs.Recipes;
using Microsoft.AspNetCore.Mvc;

namespace J3M.Controllers
{
    public class RecipeController : Controller
    {
        private readonly IAuthorizedApiClient _authorizedApiClient;

        public RecipeController(IAuthorizedApiClient authorizedApiClient)
        {
            _authorizedApiClient = authorizedApiClient;
        }

        // Initial page load
        public async Task<IActionResult> Index()
        {
            var vm = await BuildRecipeViewModel();
            return View(vm);
        }

        // Filter recipes by allergies/diets
        [HttpPost]
        public async Task<IActionResult> FilterRecipes(List<int> AllergyIds, List<int> DietIds)
        {
            var recipes = await _authorizedApiClient.PostAsync<List<RecipeDetailDto>>(
                "api/Recipes/filter",
                new RecipeFilterRequest { AllergyIds = AllergyIds, DietIds = DietIds }
            );

            var vm = await BuildRecipeViewModel(recipes, AllergyIds, DietIds);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_RecipePartial", vm);

            return View("Index", vm);
        }

        // Toggle favorite for logged-in user
        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int recipeId)
        {
            await _authorizedApiClient.PostAsync<object>(
                "api/UserRecipes/toggle",
                new { RecipeId = recipeId }
            );

            return RedirectToAction("Index");
        }

        // Helper to build the ViewModel
        private async Task<RecipeViewModel> BuildRecipeViewModel(
            List<RecipeDetailDto>? recipes = null,
            List<int>? selectedAllergyIds = null,
            List<int>? selectedDietIds = null)
        {
            var allergies = await _authorizedApiClient.GetAsync<List<AllergyDto>>("api/meta/allergies");
            var diets = await _authorizedApiClient.GetAsync<List<DietDto>>("api/meta/diets");

            return new RecipeViewModel
            {
                Recipes = recipes ?? await _authorizedApiClient.GetAsync<List<RecipeDetailDto>>("api/Recipes"),
                Allergies = allergies.Select(a => new AllergyOption
                {
                    Id = a.AllergyId,
                    Name = a.AllergyName,
                    IsSelected = selectedAllergyIds != null && selectedAllergyIds.Contains(a.AllergyId)
                }).ToList(),
                Diets = diets.Select(d => new DietOption
                {
                    Id = d.DietId,
                    Name = d.DietName,
                    IsSelected = selectedDietIds != null && selectedDietIds.Contains(d.DietId)
                }).ToList()
            };
        }
    }
}
