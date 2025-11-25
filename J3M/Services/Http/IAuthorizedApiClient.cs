using J3M.Shared.MealPlanModels;

namespace J3M.Services.Http
{
    public interface IAuthorizedApiClient
    {
        HttpClient CreateClient();
        Task<T?> GetAsync<T>(string url);
        Task<T?> PostAsync<T>(string url, object body);
        Task<T?> SendAsync<T>(HttpMethod method, string url, object? body = null);
    }
}
