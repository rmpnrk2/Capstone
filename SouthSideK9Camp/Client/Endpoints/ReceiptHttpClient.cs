using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace SouthSideK9Camp.Client.Endpoints;

public class ReceiptHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public ReceiptHttpClient(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    // GET from database
    public async Task<List<Shared.Receipt>> GetAsync()
    {
        List<Shared.Receipt> receipts = await _httpClient.GetFromJsonAsync<List<Shared.Receipt>>("receipt") ?? new List<Shared.Receipt>();
        await _localStorage.SetItemAsync("receipts", receipts);
        return receipts;
    }
    // Load from database
    public async Task<List<Shared.Receipt>> LoadAsync() =>
        await _localStorage.GetItemAsync<List<Shared.Receipt>>("receitps") ?? await GetAsync();
}
