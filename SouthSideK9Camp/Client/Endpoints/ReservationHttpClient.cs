using Blazored.LocalStorage;
using System.Net.Http.Json;
using System.Net;
using SouthSideK9Camp.Shared;

namespace SouthSideK9Camp.Client.Endpoints;

public class ReservationHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public ReservationHttpClient(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    // fetch
    public async Task FetchAsync()
    {
        List<Shared.Reservation> reservations = await _httpClient.GetFromJsonAsync<List<Shared.Reservation>>("reservations") ?? new List<Shared.Reservation>();
        await _localStorage.SetItemAsync("reservations", reservations);
    }

    // get all
    public async Task<List<Shared.Reservation>> GetAsync() =>
        await _localStorage.GetItemAsync<List<Shared.Reservation>>("reservations") ?? new();

    // get by id
    public async Task<Shared.Reservation> GetAsync(int reservationID)
    {
        List<Shared.Reservation> reservations = await GetAsync();

        return reservations.FirstOrDefault(r => r.ID == reservationID) ?? new();
    }

    // get by guid
    public async Task<Shared.Reservation?> GetByGuid(string reservationGUID)
    {
        try
        {
            Shared.Reservation? reservation = await _httpClient.GetFromJsonAsync<Shared.Reservation>($"reservations/guid/{reservationGUID}") ?? null;
            return reservation;
        }
        catch
        {
            return null;
        }
    }

    // add
    public async Task AddAsync(Shared.Reservation reservation) =>
        await _httpClient.PostAsJsonAsync($"reservations", reservation);

    // update
    public async Task UpdateAsync(Shared.Reservation reservation) =>
        await _httpClient.PutAsJsonAsync($"reservations", reservation);

    // delete
    public async Task DeleteAsync(int id) =>
        await _httpClient.DeleteAsync($"reservations/{id}");
}
