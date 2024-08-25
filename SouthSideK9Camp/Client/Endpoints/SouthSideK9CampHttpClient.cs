using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace SouthSideK9Camp.Client.Endpoints;

public class SouthSideK9CampHttpClient
{

    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    private readonly ClientHttpClient _client;
    private readonly MemberHttpClient _member;
    private readonly MembershipDueHttpClient _membershipDue;
    private readonly ReservationHttpClient _reservation;

    public SouthSideK9CampHttpClient(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;

        _client = new(httpClient, localStorage);
        _member = new(httpClient);
        _membershipDue = new(httpClient);
        _reservation = new(httpClient, localStorage);
    }

    public ClientHttpClient Client => _client;
    public MemberHttpClient Member => _member;
    public MembershipDueHttpClient MembershipDue => _membershipDue;
    public ReservationHttpClient Reservation => _reservation;
}
