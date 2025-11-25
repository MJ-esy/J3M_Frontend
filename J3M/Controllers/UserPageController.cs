using J3M.Models;
using J3M.Services.Http;
using J3M.Shared.DTOs.Ingredients;
using J3M.Shared.DTOs.Recipes;
using J3M.Shared.DTOs.Users.ProfileDtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

public class UserPageController : Controller
{
    private readonly IAuthorizedApiClient _authorizedApiClient;


    public UserPageController(IAuthorizedApiClient authorizedApiClient)
    {
        _authorizedApiClient = authorizedApiClient;
      
    }

    // GET: /UserPage/Index
    // Loads the user's profile and favorite recipes from the backend API
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var client = _authorizedApiClient.CreateClient();
        // Request current user profile from backend
        var profileResponse = await client.GetAsync("api/Profile/me");
        if (!profileResponse.IsSuccessStatusCode)
        {
            // If backend rejects JWT, redirect to login
            return RedirectToAction("Login", "Account");
        }

        var profile = await profileResponse.Content.ReadFromJsonAsync<UserProfileDto>()!;

        // Request user's favorite recipes from backend
        var favoritesResponse = await client.GetAsync("api/UserRecipes/favorites");
        var favorites = favoritesResponse.IsSuccessStatusCode
            ? await favoritesResponse.Content.ReadFromJsonAsync<List<RecipeDetailDto>>()
            : new List<RecipeDetailDto>();

        var vm = new UserProfileViewModel
        {
            FullName = profile?.UserName ?? "Unknown",
            Email = profile?.Email ?? "",
            SavedRecipes = favorites,
            Profile = profile
        };

        return View(vm);
    }

    // GET: /UserPage/Ingredients
    // Returns a partial view with an empty ingredient list
    [HttpGet]
    public IActionResult Ingredients()
    {
        // Start with an empty list
        var ingredients = new List<string>();
        return PartialView("_Ingredient", ingredients);
    }

    // POST: /UserPage/AddIngredient
    // Adds a new ingredient to the list (avoids duplicates)
    [HttpPost]
    [IgnoreAntiforgeryToken]
    public IActionResult AddIngredient([FromBody] IngredientListRequest request)
    {
        var updated = request.CurrentIngredients?.Distinct(StringComparer.OrdinalIgnoreCase).ToList()
                      ?? new List<string>();

        TempData["CurrentIngredients"] = updated; // Store updated list temporarily in TempData
        return PartialView("_Ingredient", updated); // Return updated ingredient list partial view
    }

    // POST: /UserPage/RemoveIngredient
    // Removes a specific ingredient from the list
    [HttpPost]
    [IgnoreAntiforgeryToken]
    public IActionResult RemoveIngredient([FromBody] IngredientRequest request)
    {
        var list = request.CurrentIngredients ?? new List<string>();
        if (!string.IsNullOrWhiteSpace(request.Ingredient))
            list.Remove(request.Ingredient);

        return PartialView("_Ingredient", list);
    }


    // POST: /UserPage/FilterRecipes
    // Filters recipes based on user-provided ingredients
    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> FilterRecipes([FromBody] List<string> userIngredients)
    {
        var client = _authorizedApiClient.CreateClient();
        var response = await client.PostAsJsonAsync("api/Recipes/filter", userIngredients); // Send ingredients to backend API for filtering

        if (!response.IsSuccessStatusCode)
            return PartialView("_FilteredRecipes", new List<RecipeDetailDto>());

        var recipes = await response.Content.ReadFromJsonAsync<List<RecipeDetailDto>>() ?? new List<RecipeDetailDto>();
        return PartialView("_FilteredRecipes", recipes);
    }

    // GET: /UserPage/RecipeDetails/{id}
    // Loads detailed information for a single recipe
    [HttpGet]
    public async Task<IActionResult> RecipeDetails(int id)
    {
        var client = _authorizedApiClient.CreateClient();
        var recipe = await client.GetFromJsonAsync<RecipeDetailDto>($"api/Recipes/{id}");

        if (recipe == null)
            return PartialView("_RecipeDetails", new RecipeDetailDto());

        return PartialView("_RecipeDetails", recipe);
    }



}
