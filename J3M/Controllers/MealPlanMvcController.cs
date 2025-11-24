using J3M.Shared.MealPlanModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace J3m_BE.Controllers
{
    public class MealPlanMvcController : Controller
    {
        private readonly HttpClient _http;

        public MealPlanMvcController(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient("J3MApi");
        }

        // GET: /MealPlanMvc/
        public IActionResult Index()
        {
            return View();
        }

        // POST: /MealPlanMvc/GenerateWeeklyMenu
        [HttpPost]
        public async Task<IActionResult> GenerateWeeklyMenu(MealPlanRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/MealPlan/weekly/ai", request);
            var plan = await response.Content.ReadFromJsonAsync<WeeklyMealPlanDto>();
            return View("Index", plan);
        }
    }
}
