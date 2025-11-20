namespace J3M.Services.Http
{
    public interface IAuthorizedApiClient
    {
        HttpClient CreateClient();
    }
}
