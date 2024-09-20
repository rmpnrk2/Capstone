using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace SouthSideK9Camp.Client.Endpoints;

public class DogHttpClient
{

    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public DogHttpClient(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    // fetch
    public async Task FetchAsync()
    {
        List<Shared.Dog>? dogs = await _httpClient.GetFromJsonAsync<List<Shared.Dog>>("dog");
        await _localStorage.SetItemAsync("dogs", dogs);
        return;
    }

    // get all
    public async Task<List<Shared.Dog>> GetAsync() =>
        await _localStorage.GetItemAsync<List<Shared.Dog>>("dogs") ?? new();

    // get by id
    public async Task<Shared.Dog> GetByIDAsync(int dogID)
    {
        List<Shared.Dog>? dogs = await _localStorage.GetItemAsync<List<Shared.Dog>>("dogs");
        if(dogs == null) return new();
        return dogs.FirstOrDefault(d => d.ID == dogID) ?? new();
    }

    // get by guid
    public async Task<Shared.Dog?> GetByGuid(string dogGUID)
    {
        try
        {
            Shared.Dog? dog = await _httpClient.GetFromJsonAsync<Shared.Dog>($"dog/guid/{dogGUID}") ?? null;
            return dog;
        }
        catch
        {
            return null;
        }
    }

    // update
    public async Task UpdateAsync(Shared.Dog dog)
    {
        await _httpClient.PutAsJsonAsync($"dog/{dog.ID}", dog);
        await _httpClient.PutAsJsonAsync($"contracts/{dog.Contract.ID}", dog.Contract);
    }

    // upload dog avatar
    public async Task UploadAvatar(int dogID, MultipartFormDataContent multipartContent) =>
        await _httpClient.PostAsync($"dog/upload-avatar/{dogID}", multipartContent);

    // delete
    public async Task DeleteAsync(int dogID)
    {
        await _httpClient.DeleteAsync($"dog/{dogID}");
    }
}
