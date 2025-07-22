namespace PatientPortal.Services;

public interface IApiHttpClient
{
    Task<T?> GetAsync<T>(string endpoint);
    Task<T?> PutAsync<T>(string endpoint, object data);
    Task<T?> PostAsync<T>(string endpoint, object data);

    Task DeleteAsync(string endpoint);
}