using System.Net.Http.Headers;

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
            //Create HttpClient pointing to backend API base URL
            var client = _httpClientFactory.CreateClient("J3MApi");

            // Read JWT token from cookie set at login
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];

            //If token exists, add it to the Authorization header
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }
    }
}
