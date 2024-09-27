using Blazored.LocalStorage;
using System.Net;
using System.Net.Http.Json;

namespace SouthSideK9Camp.Client.Endpoints;

public class CustomerHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public CustomerHttpClient(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    // check for email availability
    public async Task<string> CheckEmailAsync(string email)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"customer/check-email/{email}");

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return response.Content.ReadFromJsonAsync<Shared.Client>().Result?.GUID.ToString() ?? string.Empty;
        }
        return string.Empty;
    }

    // get by guid
    public async Task<Shared.Client?> GetByGuid(string clientGUID)
    {
        try
        {
            Shared.Client? client = await _httpClient.GetFromJsonAsync<Shared.Client>($"customer/guid/{clientGUID}") ?? null;
            return client;
        }
        catch
        {
            return null;
        }
    }

    // register customer
    public async Task Register(string reservationGUID, Shared.Client client) =>
        await _httpClient.PostAsJsonAsync($"customer/register/{reservationGUID}", client);

    // reservation payment
    public async Task ReservationPayment(int dogID, MultipartFormDataContent multipartContent) =>
        await _httpClient.PostAsync($"customer/reservation-payment/{dogID}", multipartContent);
    
    // reservation payment resubmit
    public async Task ReservationPaymentResubmit(int dogID) =>
        await _httpClient.PostAsync($"customer/reservation-payment-resubmit/{dogID}", null);

    // update
    public async Task UpdateAsync(Shared.Customer customer) =>
        await _httpClient.PutAsJsonAsync($"customer/{customer.ID}", customer);

    // approve payment
    public async Task ApprovePaymentAsync(int dogID) =>
        await _httpClient.PutAsync($"customer/payment-approve/{dogID}", null);

    // reject payment
    public async Task RejectPaymentAsync(int dogID, Shared.ReasonForRejection reason) =>
        await _httpClient.PutAsJsonAsync($"customer/payment-reject/{dogID}", reason);

    // delete
    public async Task DeleteAsync(int customerID) =>
        await _httpClient.DeleteAsync($"customer/{customerID}");
}
