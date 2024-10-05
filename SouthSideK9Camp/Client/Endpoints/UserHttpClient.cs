using Blazored.LocalStorage;
using System.Net.Http.Json;
using Blazored.SessionStorage;

namespace SouthSideK9Camp.Client.Endpoints;

public class UserHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly ISessionStorageService _sessionStorage;

    public UserHttpClient(HttpClient httpClient, ILocalStorageService localStorage, ISessionStorageService sessionStorageService)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _sessionStorage = sessionStorageService;
    }

    // Fetch User database and save to browser localstorage
    public async Task<List<Shared.User>> FetchAsync()
    {
        List<Shared.User> users = await _httpClient.GetFromJsonAsync<List<Shared.User>>("users") ?? new List<Shared.User>();
        await _localStorage.SetItemAsync("users", users);
        return users;
    }

    // Get user data from browser localstorage
    public async Task<List<Shared.User>> GetAsync() =>
        await _localStorage.GetItemAsync<List<Shared.User>>("users") ?? await FetchAsync();

    // Get User using User.ID
    public async Task<Shared.User?> GetByIDAsync(int userID) =>
        await _httpClient.GetFromJsonAsync<Shared.User>($"users/{userID}") ?? null;
    
    // Check current User login status
    public async Task<Shared.User?> LoginStatusAsync()
    {
        return await _sessionStorage.GetItemAsync<Shared.User?>("user");
    }

    // Check login credential (login attemt), if credential is valid save User to sessionstorage
    public async Task<Shared.User?> LoginAsync(string username, string password)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"users/login/{username}/{password}");

        if(response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

        Shared.User? user =  await response.Content.ReadFromJsonAsync<Shared.User>();
        if(user == null) return null;

        await _sessionStorage.SetItemAsync<Shared.User>("user", user);
        return user;
    }

    // (Logout), clear sessionstorage
    public async Task LogoutAsync() => 
        await _sessionStorage.RemoveItemAsync("user"); 

    // Create new User, return true if created successfully
    public async Task<bool> CreateAsync(Shared.User user)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"users", user);

        if(response.StatusCode == System.Net.HttpStatusCode.Conflict)
            return false;

        return true;
    }

    // Update User
    public async Task<bool> UpdateAsync(Shared.User user)
    {
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"users/{user.ID}", user);

        if(response.StatusCode == System.Net.HttpStatusCode.Conflict)
            return false;

        return true;
    }

    // Delete User
    public async Task DeleteAsync(int id) =>
        await _httpClient.DeleteAsync($"users/{id}");
}
