using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using J3M.Shared.MealPlanModels;

namespace J3M.Services.Http
{
    public class AuthorizedApiClient : IAuthorizedApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizedApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient("J3MApi");

            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        // Generic GET
        public async Task<T?> GetAsync<T>(string url)
        {
            var client = CreateClient();
            return await client.GetFromJsonAsync<T>(url);
        }

        // Generic POST
        public async Task<T?> PostAsync<T>(string url, object body)
        {
            var client = CreateClient();
            var response = await client.PostAsJsonAsync(url, body);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        // Generic Send (for arbitrary HttpMethod)
        public async Task<T> SendAsync<T>(HttpMethod method, string url, object? body = null)
        {
            var client = CreateClient();
            var request = new HttpRequestMessage(method, url);

            if (body != null)
            {
                request.Content = JsonContent.Create(body);
            }

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
