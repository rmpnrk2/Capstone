using SouthSideK9Camp.Shared;
using System.Net.Http.Json;

namespace SouthSideK9Camp.Client.Endpoints;


public class MembershipDueHttpClient
{
    private readonly HttpClient _httpClient;

    public MembershipDueHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    // get by guid
    public async Task<Shared.Client?> GetByGuid(string membershipGUID)
    {
	    try
	    {
            Shared.Client? client = await _httpClient.GetFromJsonAsync<Shared.Client>($"membership-dues/guid/{membershipGUID}") ?? null;
            return client;
        }
        catch
        {
            return null;
        }
    }

    // membershipdue payment
    public async Task DuePaymentAsync(int membershipDueID, MultipartFormDataContent multipartContent) =>
        await _httpClient.PostAsync($"membership-dues/payment/{membershipDueID}", multipartContent);
    
    // membershipdue payment resubmit
    public async Task DuePaymentResubmitAsync(int membershipDueID) =>
        await _httpClient.PutAsync($"membership-dues/payment-resubmit/{membershipDueID}", null);

    // approve membershipdue payment
    public async Task ApprovePaymentAsync(int membershipDueID) =>
        await _httpClient.PutAsync($"membership-dues/payment-approve/{membershipDueID}", null);

    // reject membershipdue payment
    public async Task RejectPaymentAsync(int membershipDueID, Shared.ReasonForRejection reason) =>
        await _httpClient.PutAsJsonAsync($"membership-dues/payment-reject/{membershipDueID}", reason);

    // notify membershipdue
    public async Task NotifyAsync() =>
        await _httpClient.PutAsync($"membership-dues/payment-notify", null);

    // delete
    public async Task DeleteAsync(int memberID) =>
        await _httpClient.DeleteAsync($"membership-dues/{memberID}");
}
