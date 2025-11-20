using J3M.Services.Http;
using J3m_BE.DTOs.Users;       
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace J3M.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            //Return empty LoginDto to the view
            return View(new LoginDto());
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            //Let MVC handle client-side & server-side validation run
            if (!ModelState.IsValid)
                return View(model);

            //Create HttpClient that points to the backend API
            var client = _httpClientFactory.CreateClient("J3MApi");

            // Send login credentials to backend AuthController
            //  Backend-route: [Route("api/[controller]")] + [HttpPost("login")]
            var response = await client.PostAsJsonAsync("api/Auth/login", model);

            if (!response.IsSuccessStatusCode)
            {
                //If backend says 400/401 -> invalid credentials
                ModelState.AddModelError(string.Empty, "Felaktigt användarnamn eller lösenord.");
                return View(model);
            }

            // Read the AuthResponseDto returned by AuthService
            var loginResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            //Safety check: backend should always send token + expiry. Guard against null/empty tp avoid null reference issues
            if (loginResponse == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                ModelState.AddModelError(string.Empty, "Ogiltigt svar från servern.");
                return View(model);
            }

            // Store JWT token in an HttpOnly cookie so it cannot be read by JavaScript.
            // Token later be attached as Authorization header by AuthorizationApiClient.
            Response.Cookies.Append("AuthToken", loginResponse.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,                 // true i prod/https
                SameSite = SameSiteMode.Strict,
                Expires = loginResponse.ExpiresAtUtc.ToLocalTime()
            });

            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Remove JWT-cookie
            Response.Cookies.Delete("AuthToken");

            // Clear all session data after user logs out
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
