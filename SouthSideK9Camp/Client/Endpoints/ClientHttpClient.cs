using Blazored.LocalStorage;
using MudBlazor;
using System.Net;
using System.Net.Http.Json;

namespace SouthSideK9Camp.Client.Endpoints;

public class ClientHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public ClientHttpClient(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    // fetch
    public async Task<List<Shared.Client>> FetchAsync()
    {
        List<Shared.Client> clients = await _httpClient.GetFromJsonAsync<List<Shared.Client>>("clients") ?? new List<Shared.Client>();
        await _localStorage.SetItemAsync("clients", clients);
        return clients;
    }

    // get all
    public async Task<List<Shared.Client>> GetAsync() =>
        await _localStorage.GetItemAsync<List<Shared.Client>>("clients") ?? await FetchAsync();

    // get by id
    public async Task<Shared.Client?> GetByIDAsync(int id) =>
        await _httpClient.GetFromJsonAsync<Shared.Client>($"clients/{id}") ?? null;

    // check for email availability when registering as member
    public async Task<bool> CheckCustomerEmailAvailabilityAsync(string email)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"clients/check-customer-email-availability/{email}");

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return true;
        }
        return false;
    }

    // check for email availability when registering as member
    public async Task<bool> CheckMemberEmailAvailabilityAsync(string email)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"clients/check-member-email-availability/{email}");

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return true;
        }
        return false;
    }

    // register customer
    public async Task RegisterCustomer(Shared.Client client) =>
        await _httpClient.PostAsJsonAsync($"clients/customer-registration/", client);

    // register member
    public async Task RegisterMember(Shared.Client client) =>
        await _httpClient.PostAsJsonAsync($"clients/member-registration/", client);

    // update
    public async Task UpdateAsync(Shared.Client client) =>
        await _httpClient.PutAsJsonAsync($"clients/{client.ID}", client);

    // delete
    public async Task DeleteAsync(int id) =>
        await _httpClient.DeleteAsync($"clients/{id}");

}