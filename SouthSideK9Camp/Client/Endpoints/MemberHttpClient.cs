using System.Net.Http.Json;

namespace SouthSideK9Camp.Client.Endpoints;

public class MemberHttpClient
{
    private readonly HttpClient _httpClient;

    public MemberHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // get by guid
    public async Task<Shared.Client?> GetByGuid(string memberGUID)
    {
        try
        {
            Shared.Client? client = await _httpClient.GetFromJsonAsync<Shared.Client>($"members/guid/{memberGUID}") ?? null;
            return client;
        }
        catch
        {
            return null;
        }
    }

    // pay membership registration payment
    public async Task RegistrationPayment(int memberID, MultipartFormDataContent multipartContent) =>
        await _httpClient.PostAsync($"members/registration-payment/{memberID}", multipartContent);
    
    // pay membership registration payment resubmit
    public async Task RegistrationPaymentResubmit(int memberID) =>
        await _httpClient.PostAsync($"members/registration-payment-resubmit/{memberID}", null);

    // update
    public async Task UpdateAsync(Shared.Client client) =>
        await _httpClient.PutAsJsonAsync($"members/{client.ID}", client);

    // approve membership payment
    public async Task ApprovePaymentAsync(int clientID) =>
        await _httpClient.PutAsync($"members/payment-approve/{clientID}", null);

    // reject membership payment
    public async Task RejectPaymentAsync(int clientID) =>
        await _httpClient.PutAsync($"members/payment-reject/{clientID}", null);

    // delete
    public async Task DeleteAsync(int memberID) =>
        await _httpClient.DeleteAsync($"members/{memberID}");

}
