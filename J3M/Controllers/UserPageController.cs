using J3M.Models;
using J3M.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using J3m_BE.DTOs.Recipes;

namespace J3m_FE.Controllers
{
    [Authorize]
    public class UserPageController : Controller
    {
        private readonly HttpClient _httpClient;

        public UserPageController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BackendApi");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _httpClient.GetAsync("api/UserRecipes/favorites");
            response.EnsureSuccessStatusCode();

            var favorites = await response.Content.ReadFromJsonAsync<List<RecipeDetailDto>>();

            var vm = new UserProfileViewModel
            {
                FullName = User.Identity?.Name ?? "Unknown",
                Email = User.FindFirstValue(ClaimTypes.Email) ?? "",
                SavedRecipes = favorites ?? new List<RecipeDetailDto>()
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> RecipeDetails(int id)
        {
            var response = await _httpClient.GetAsync($"api/UserRecipes/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var recipe = await response.Content.ReadFromJsonAsync<RecipeDetailDto>();
            return PartialView("_RecipeDetails", recipe);
        }
    }
}
