using J3M.DTOs;
using J3M.Models;
using J3M.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;


namespace J3m_FE.Controllers
{
    public class UserPageController : Controller
    {
        [Authorize]
        public class UserPageController : Controller
        {
            private readonly IUserRecipeService _userRecipeService;

            public UserPageController(IUserRecipeService userRecipeService)
            {
                _userRecipeService = userRecipeService;
            }

            [HttpGet]
            public async Task<IActionResult> Dashboard()
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var favorites = await _userRecipeService.GetFavoriteRecipesAsync(userId);

                var vm = new UserProfileViewModel
                {
                    FullName = User.Identity?.Name ?? "Unknown",
                    Email = User.FindFirstValue(ClaimTypes.Email) ?? "",
                    SavedRecipes = favorites.ToList()
                };

                return View(vm);
            }
        }
    }
}


    