using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace AlzaBoxApiTests;

public class AlzaBoxApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AlzaBoxApiClient(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient();
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var formData = new Dictionary<string, string>
        {
            {"grant_type", "client_credentials"},
            {"client_id", _configuration["ApiSettings:ClientId"]},
            {"client_secret", _configuration["ApiSettings:ClientSecret"]}
        };

        var response = await _httpClient.PostAsync(_configuration["ApiSettings:AuthUrl"], new FormUrlEncodedContent(formData));
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JObject.Parse(content)["access_token"].ToString();
    }

    public async Task<JObject> GetAsync(string endpoint)
    {
        var token = await GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync($"{_configuration["ApiSettings:ApiBaseUrl"]}/{endpoint}");
        response.EnsureSuccessStatusCode();
        return JObject.Parse(await response.Content.ReadAsStringAsync());
    }

    public async Task<JObject> PostAsync(string endpoint, object data)
    {
        var token = await GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_configuration["ApiSettings:ApiBaseUrl"]}/{endpoint}", content);
        return JObject.Parse(await response.Content.ReadAsStringAsync());
    }
}
