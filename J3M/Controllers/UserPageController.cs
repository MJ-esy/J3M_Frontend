using J3M.Models;
using J3M.Services.Http;
using J3M.Shared.DTOs.Ingredients;
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

    [HttpGet]
    public IActionResult Ingredients()
    {
        // Start with an empty list
        var ingredients = new List<string>();
        return PartialView("_Ingredient", ingredients);
    }
    [HttpPost]
    [IgnoreAntiforgeryToken] // AJAX requests do not send token
    public IActionResult AddIngredient([FromBody] IngredientRequest request)
    {
        var list = request.CurrentIngredients ?? new List<string>();
        if (!string.IsNullOrWhiteSpace(request.Ingredient))
            list.Add(request.Ingredient);

        return PartialView("_Ingredient", list);
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public IActionResult RemoveIngredient([FromBody] IngredientRequest request)
    {
        var list = request.CurrentIngredients ?? new List<string>();
        if (!string.IsNullOrWhiteSpace(request.Ingredient))
            list.Remove(request.Ingredient);

        return PartialView("_Ingredient", list);
    }

    //[HttpPost]
    //public IActionResult AddIngredient([FromBody] IngredientRequest request)
    //{
    //    var currentIngredients = request.CurrentIngredients ?? new List<string>();

    //    if (!string.IsNullOrWhiteSpace(request.Ingredient))
    //        currentIngredients.Add(request.Ingredient);

    //    return PartialView("_Ingredient", currentIngredients);
    //}

    //[HttpPost]
    //[IgnoreAntiforgeryToken] // important for AJAX
    //public IActionResult RemoveIngredient([FromBody] IngredientRequest request)
    //{
    //    var list = request.CurrentIngredients ?? new List<string>();
    //    if (!string.IsNullOrWhiteSpace(request.Ingredient))
    //        list.Remove(request.Ingredient);

    //    return PartialView("_Ingredient", list);
    //}


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
