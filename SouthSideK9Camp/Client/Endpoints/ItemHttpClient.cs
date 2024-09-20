using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace SouthSideK9Camp.Client.Endpoints;

public class ItemHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;


    public ItemHttpClient(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    // fetch models
    public async Task FetchModelAsync()
    {
        List<Shared.Item>? items = await _httpClient.GetFromJsonAsync<List<Shared.Item>>("item/models");
        await _localStorage.SetItemAsync("defaultItems", items);
        return;
    }

    // get models
    public async Task<List<Shared.Item>> GetModelAsync()
    {
        List<Shared.Item>? items = await _localStorage.GetItemAsync<List<Shared.Item>>("defaultItems");
        return items ?? new();
    }

    // get by id
    public async Task<Shared.Item> GetByIDAsync(int itemID)
    {
        List<Shared.Item>? items = await _localStorage.GetItemAsync<List<Shared.Item>>("defaultItems");
        if(items == null) return new();
        return items.FirstOrDefault(i => i.ID == itemID) ?? new();
    }

    // add
    public async Task CreateAsync(int invoiceID, Shared.Item item) =>
        await _httpClient.PostAsJsonAsync($"item/{invoiceID}", item);

    // create model
    public async Task CreateModelAsync(Shared.Item item) =>
        await _httpClient.PostAsJsonAsync($"item/model", item);

    // update
    public async Task UpdateAsync(Shared.Item item) =>
        await _httpClient.PutAsJsonAsync($"item/{item.ID}", item);

    // delete
    public async Task DeleteAsync(int itemID) =>
        await _httpClient.DeleteAsync($"item/{itemID}");
}
