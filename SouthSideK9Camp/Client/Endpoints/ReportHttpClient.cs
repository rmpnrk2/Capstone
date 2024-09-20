using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace SouthSideK9Camp.Client.Endpoints;


public class ReportHttpClient
{
    private readonly HttpClient _httpClient;

    public ReportHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // add
    public async Task PostAsync(int dogID, MultipartFormDataContent multipartContent) =>
        await _httpClient.PostAsync($"report/{dogID}", multipartContent);

    // update
    public async Task UpdateAsync(int dogID, MultipartFormDataContent multipartContent) =>
        await _httpClient.PutAsync($"report/{dogID}", multipartContent);

    // delete
    public async Task DeleteAsync(int dogID) =>
        await _httpClient.DeleteAsync($"report/{dogID}");
}