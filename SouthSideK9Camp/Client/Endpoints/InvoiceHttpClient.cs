using Blazored.LocalStorage;
using System.Net.Http.Json;
using System.Net;

namespace SouthSideK9Camp.Client.Endpoints;

public class InvoiceHttpClient
{
    private readonly HttpClient _httpClient;

    public InvoiceHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // get default
    public async Task<Shared.Invoice> GetDefaultAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("invoice/default");

        if (response.StatusCode == HttpStatusCode.OK)
            return response.Content.ReadFromJsonAsync<Shared.Invoice>().Result ?? new();
        
        return new();
    }

    // get by guid
    public async Task<Shared.Invoice> GetByGuid(string invoiceGUID)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"invoice/guid/{invoiceGUID}");

        if (response.StatusCode == HttpStatusCode.OK)
            return response.Content.ReadFromJsonAsync<Shared.Invoice>().Result ?? new();
        
        return new();
    }

    // add
    public async Task CreateAsync(int dogID, Shared.Invoice invoice) =>
        await _httpClient.PostAsJsonAsync($"invoice/{dogID}", invoice);

    // create default
    public async Task CreateDefaultAsync(Shared.Invoice invoice) =>
        await _httpClient.PostAsJsonAsync($"invoice/default", invoice);

    // update
    public async Task UpdateAsync(Shared.Invoice invoice) =>
        await _httpClient.PutAsJsonAsync($"invoice/{invoice.ID}", invoice);

    // invoice payment
    public async Task PaymentAsync(int invoiceID, MultipartFormDataContent multipartContent) =>
        await _httpClient.PutAsync($"invoice/payment/{invoiceID}", multipartContent);
    
    // invoice payment resubmit
    public async Task PaymentResubmitAsync(int invoiceID) =>
        await _httpClient.PutAsync($"invoice/payment-resubmit/{invoiceID}", null);

    // approve payment
    public async Task ApprovePaymentAsync(int invoiceID) =>
        await _httpClient.PutAsync($"invoice/payment-approve/{invoiceID}", null);

    // reject payment
    public async Task RejectPaymentAsync(int invoiceID, Shared.ReasonForRejection reason) =>
        await _httpClient.PutAsJsonAsync($"invoice/payment-reject/{invoiceID}", reason);

    // email invoice
    public async Task EmailAsync(int invoiceID) =>
        await _httpClient.PutAsync($"invoice/sendemail/{invoiceID}", null);

    // delete
    public async Task DeleteAsync(int invoiceID) =>
        await _httpClient.DeleteAsync($"invoice/{invoiceID}");
}
