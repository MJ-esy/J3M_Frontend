using J3M.Models;
using J3M.Services.Http;
using J3m_BE.DTOs.Recipes;
using J3m_BE.DTOs.Users.ProfileDtos;
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
}
