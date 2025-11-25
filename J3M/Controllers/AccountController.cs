using J3M.Services.Http;
using J3M.Shared.DTOs.Users.AuthDtos;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto model)
        {
            // Låt MVC köra DataAnnotations-validiering
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("J3MApi");

            var response = await client.PostAsJsonAsync("api/Auth/login", model);

            if (!response.IsSuccessStatusCode)
            {
                // Försök läsa ProblemDetails från backend (ErrorHandlingMiddleware)
                var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

                if (problem != null)
                {
                    // Backend sätter Detail = "Invalid credentials."
                    var backendMessage = problem.Detail ?? problem.Title ?? "Login failed.";

                    // Lägg in direkt i ModelState så ValidationSummary visar det
                    ModelState.AddModelError(string.Empty, backendMessage);
                }
                else
                {
                    // Om backend inte skickar ProblemDetails av någon anledning
                    ModelState.AddModelError(string.Empty, "Login failed. Please check your credentials.");
                }

                return View(model);
            }

            // Om vi kommer hit har login lyckats
            var loginResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            if (loginResponse == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                ModelState.AddModelError(string.Empty, "Invalid response from server.");
                return View(model);
            }

            Response.Cookies.Append("AuthToken", loginResponse.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = loginResponse.ExpiresAtUtc.ToLocalTime()
            });

            return RedirectToAction("Index", "UserPage"); // Redirect to Personal userpage
   
        }
        

        // Add this GET action to allow GET /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            // Return empty RegisterDto to the view so the Register.cshtml model binding works
            return View(new RegisterDto());
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("J3MApi");

            // Send registration data to backend AuthController
            // Backend-route: [Route("api/[controller]")] + [HttpPost("register")]
            var response = await client.PostAsJsonAsync("api/Auth/register", model);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Registration failed: {errorContent}");
                return View(model);
            }
            return RedirectToAction("Login", "Account");
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