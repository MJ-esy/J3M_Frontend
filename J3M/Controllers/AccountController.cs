using J3M.Shared.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;


namespace J3M.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginDto());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var client = _httpClientFactory.CreateClient("J3MApi");

            //Sends login to our backend WebAPI
            var response = await client.PostAsJsonAsync("api/auth/login", model);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            var loginResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            if (loginResponse == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                ModelState.AddModelError(string.Empty, "Invalid login response.");
                return View(model);
            }

            // Store the token in a cookie
            Response.Cookies.Append("AuthToken", loginResponse.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = loginResponse.ExpiresAtUtc.ToLocalTime()
            });

            HttpContext.Session.SetString("UserRole", loginResponse.Token);

            return RedirectToAction("Index", "Home");

        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
