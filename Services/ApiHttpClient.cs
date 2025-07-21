using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PatientPortal.Services;

public class ApiHttpClient : IApiHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;


    public ApiHttpClient(HttpClient httpClient, ITokenService tokenService, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
        _configuration = configuration;

        _httpClient.BaseAddress = new Uri("https://localhost:7047/api/");
    }

    private async Task<HttpClient> GetAuthenticatedClientAsync()
    {
        var token = await _tokenService.GetAccessTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        return _httpClient;
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        var client = await GetAuthenticatedClientAsync();

        var response = await client.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
    public async Task<T?> PostAsync<T>(string endpoint, object data)
    {
        var client = await GetAuthenticatedClientAsync();
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(responseJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true

        });

    }
    public async Task<T?> PutAsync<T>(string endpoint, object data)
    {
        var client = await GetAuthenticatedClientAsync();
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PutAsync(endpoint, content);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(responseJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true

        });

    }
    public async Task DeleteAsync(string endpoint)
    {
        var client = await GetAuthenticatedClientAsync();
        var response = await client.DeleteAsync(endpoint);
        response.EnsureSuccessStatusCode();
    }

}