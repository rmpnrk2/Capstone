using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace SouthSideK9Camp.Client.Endpoints;

public class LogHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public LogHttpClient(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    // fetch
    public async Task FetchAsync()
    {
        List<Shared.Log>? logs = await _httpClient.GetFromJsonAsync<List<Shared.Log>>("logs");
        await _localStorage.SetItemAsync("logs", logs);
        return;
    }

    // get all
    public async Task<List<Shared.Log>> GetAsync() =>
        await _localStorage.GetItemAsync<List<Shared.Log>>("logs") ?? new();
}
