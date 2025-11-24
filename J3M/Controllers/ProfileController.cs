using J3M.Services.Http;
using J3M.Shared.DTOs.Users.ProfileDtos;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;

namespace J3M.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IAuthorizedApiClient _apiClient;

        public ProfileController(IAuthorizedApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        // GET: /Profile/Me
        public async Task<IActionResult> Me()
        {
            var client = _apiClient.CreateClient();

            // Backend: [HttpGet("me")] => api/Profile/me
            var response = await client.GetAsync("api/Profile/me");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // No valid token -> force login
                return RedirectToAction("Login", "Account");
            }

            var profile = await response.Content.ReadFromJsonAsync<UserProfileDto>();

            return View(profile);
        }

        // GET: /Profile/Edit
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var client = _apiClient.CreateClient();

            // Reuse the /me endpoint to prefill the form with current values
            var response = await client.GetAsync("api/Profile/me");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Login", "Account");

            var profile = await response.Content.ReadFromJsonAsync<UserProfileDto>();

            if (profile == null)
                return RedirectToAction(nameof(Me));

            // Map UserProfileDto -> UpdateProfileDto if needed.
            // Assuming shapes are similar, we can do:
            var updateDto = new UpdateProfileDto
            {
                UserName = profile.UserName,
                Email = profile.Email,
                DisplayName = profile.DisplayName
            };

            return View(updateDto);
        }

        // POST: /Profile/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProfileDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var client = _apiClient.CreateClient();

            // Backend: [HttpPut("me")] => api/Profile/me
            var response = await client.PutAsJsonAsync("api/Profile/me", dto);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Failed to update profile.");
                return View(dto);
            }

            return RedirectToAction(nameof(Me));
        }

        // GET: /Profile/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            // Return empty DTO to the view
            return View(new ChangePasswordDto());
        }

        // POST: /Profile/ChangePassword
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var client = _apiClient.CreateClient();

            // Backend: [HttpPut("change/password")] => api/Profile/change/password
            var response = await client.PutAsJsonAsync("api/Profile/change/password", dto);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Failed to change password.");
                return View(dto);
            }

            return RedirectToAction(nameof(Me));
        }

        // POST: /Profile/DeleteAccount
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount()
        {
            var client = _apiClient.CreateClient();

            // Backend: [HttpDelete] => api/Profile
            var response = await client.DeleteAsync("api/Profile");

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Failed to delete account.");
                return RedirectToAction(nameof(Me));
            }

            // On successful delete, log user out on frontend as well
            Response.Cookies.Delete("AuthToken");
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}

