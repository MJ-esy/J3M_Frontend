using J3M.Models;
using J3M.Services.Http;
using J3M.Shared.DTOs.Allergies;
using J3M.Shared.DTOs.Diets;
using J3M.Shared.MealPlanModels;
using Microsoft.AspNetCore.Mvc;

// Controller for Meal Plan MVC views
namespace J3M_BE.Controllers
{
    public class MealPlanMvcController : Controller
    {
        private readonly IAuthorizedApiClient _authorizedApiClient;
        private List<int>? selectedAllergyIds; // To track selected allergies
        private List<int>? selectedDietIds; // To track selected diets

        public MealPlanMvcController(IAuthorizedApiClient authorizedApiClient)
        {
            _authorizedApiClient = authorizedApiClient;
        }

        // Helper method to build the MealPlanViewModel with optional parameters
        private async Task<MealPlanViewModel> BuildViewModel(WeeklyMealPlanDto? plan = null, List<int>? selectedAllergyIds = null, List<int>? selectedDietIds = null)
        {
            var allergies = await _authorizedApiClient.GetAsync<List<AllergyDto>>("api/meta/allergies");
            var diets = await _authorizedApiClient.GetAsync<List<DietDto>>("api/meta/diets");
            return new MealPlanViewModel
            {
                WeeklyPlan = plan,
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

        // GET: MealPlanMvc/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = await BuildViewModel();
            return View(vm);
        }

        // POST: MealPlanMvc/GenerateWeeklyMenu
        //[HttpPost]
        //public async Task<IActionResult> GenerateWeeklyMenu(List<int> AllergyIds, List<int> DietIds)
        //{
        //    try
        //    {
        //        var plan = await _authorizedApiClient.PostAsync<WeeklyMealPlanDto>(
        //            "api/MealPlan/weekly/ai",
        //            new MealPlanRequest { AllergyIds = AllergyIds, DietIds = DietIds }
        //        );

        //        var vm = await BuildViewModel(plan, AllergyIds, DietIds);

        //        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //        {
        //            return PartialView("_MealPlanPartial", vm);
        //        }

        //        return View("Index", vm);
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("Error", new ErrorViewModel { Message = "Unable to generate weekly meal plan." });
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> GenerateWeeklyMenu(List<int> AllergyIds, List<int> DietIds)
        {
            var plan = await _authorizedApiClient.PostAsync<WeeklyMealPlanDto>(
                "api/MealPlan/weekly/ai",
                new MealPlanRequest { AllergyIds = AllergyIds, DietIds = DietIds }
            );

            if (plan == null)
            {
                return View("Error", new ErrorViewModel { Message = "Unable to generate weekly meal plan." });
            }

            var vm = await BuildViewModel(plan, AllergyIds, DietIds);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_MealPlanPartial", vm);
            }

            return View("Index", vm);
        }



    }
}
