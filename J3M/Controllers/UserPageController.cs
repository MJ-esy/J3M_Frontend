using J3M.Models;
using J3M.Services.Http;
using J3m_BE.DTOs.Recipes;
using J3m_BE.DTOs.Users.ProfileDtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

public class UserPageController : Controller
{
    private readonly IAuthorizedApiClient _authorizedApiClient;


    public UserPageController(IAuthorizedApiClient authorizedApiClient)
    {
        _authorizedApiClient = authorizedApiClient;
      
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var client = _authorizedApiClient.CreateClient();

        var profileResponse = await client.GetAsync("api/Profile/me");
        if (!profileResponse.IsSuccessStatusCode)
        {
            // If backend rejects JWT, redirect to login
            return RedirectToAction("Login", "Account");
        }

        var profile = await profileResponse.Content.ReadFromJsonAsync<UserProfileDto>();

        var favoritesResponse = await client.GetAsync("api/UserRecipes/favorites");
        var favorites = favoritesResponse.IsSuccessStatusCode
            ? await favoritesResponse.Content.ReadFromJsonAsync<List<RecipeDetailDto>>()
            : new List<RecipeDetailDto>();

        var vm = new UserProfileViewModel
        {
            FullName = profile?.DisplayName ?? User.Identity?.Name ?? "Unknown",
            Email = profile?.Email ?? "",
            SavedRecipes = favorites,
            Profile = profile
        };

        return View(vm);
    }
    //[HttpGet]
    //public IActionResult Ingredients()
    //{
    //    // Start with an empty list
    //    var ingredients = new List<string>();
    //    return PartialView("_Ingredients", ingredients);
    //}

    //[HttpPost]
    //public IActionResult AddIngredient(string ingredient, List<string> currentIngredients)
    //{
    //    if (!string.IsNullOrWhiteSpace(ingredient))
    //    {
    //        currentIngredients.Add(ingredient);
    //    }
    //    return PartialView("_Ingredients", currentIngredients);
    //}

    //[HttpPost]
    //public IActionResult RemoveIngredient(string ingredient, List<string> currentIngredients)
    //{
    //    currentIngredients.Remove(ingredient);
    //    return PartialView("_Ingredients", currentIngredients);
    //}

    [HttpGet, HttpPost]
    public IActionResult AddIngredient(string ingredient, List<string> currentIngredients)
    {
        if (!string.IsNullOrWhiteSpace(ingredient))
            currentIngredients.Add(ingredient);

        return PartialView("_Ingredients", currentIngredients);
    }
    [HttpGet, HttpPost]
    public IActionResult RemoveIngredient(string ingredient, List<string> currentIngredients)
    {
        currentIngredients.Remove(ingredient);
        return PartialView("_Ingredients", currentIngredients);
    }

    [HttpPost]
    public async Task<IActionResult> FilterRecipes(List<string> userIngredients)
    {
        var client = _authorizedApiClient.CreateClient();

        var response = await client.PostAsJsonAsync("api/Recipes/filter", userIngredients);
        if (!response.IsSuccessStatusCode)
        {
            return PartialView("_FilteredRecipes", new List<RecipeDetailDto>());
        }

        var recipes = await response.Content.ReadFromJsonAsync<List<RecipeDetailDto>>();
        return PartialView("_FilteredRecipes", recipes);
    }
}
