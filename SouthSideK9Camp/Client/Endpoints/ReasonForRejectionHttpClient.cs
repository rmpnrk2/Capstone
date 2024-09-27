using System.Net.Http.Json;

namespace SouthSideK9Camp.Client.Endpoints;

public class ReasonForRejectionHttpClient
{
    private readonly HttpClient _httpClient;

    private string _apiRoute = "reason-for-rejection";

    public ReasonForRejectionHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // get by guid
    public async Task<Shared.ReasonForRejection> GetByGUIDAsync(string reasonGUID) =>
        await _httpClient.GetFromJsonAsync<Shared.ReasonForRejection>($"{_apiRoute}/guid/{reasonGUID}") ?? new();

    // create
    public async Task CreateAsync(Shared.ReasonForRejection reason) =>
        await _httpClient.PostAsJsonAsync($"{_apiRoute}/", reason);

    // delete
    public async Task DeleteAsync(int reasonID) =>
        await _httpClient.DeleteAsync($"{_apiRoute}/{reasonID}");
}