using Blazored.LocalStorage;
using SouthSideK9Camp.Shared;
using System.Net.Http.Json;

namespace SouthSideK9Camp.Client.Endpoints;

public class SouthSideK9CampHttpClient
{
    private readonly ClientHttpClient _client;
    private readonly MemberHttpClient _member;
    private readonly CustomerHttpClient _customer;
    private readonly MembershipDueHttpClient _membershipDue;
    private readonly ReservationHttpClient _reservation;
    private readonly DogHttpClient _dog;
    private readonly InvoiceHttpClient _invoice;
    private readonly ItemHttpClient _item;
    private readonly ReportHttpClient _report;
    private readonly ReasonForRejectionHttpClient _reason;

    public SouthSideK9CampHttpClient(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _client = new(httpClient, localStorage);
        _member = new(httpClient);
        _customer = new(httpClient, localStorage);
        _membershipDue = new(httpClient);
        _reservation = new(httpClient, localStorage);
        _dog = new(httpClient, localStorage);
        _invoice = new(httpClient);
        _item = new(httpClient, localStorage);
        _report = new(httpClient);
        _reason = new(httpClient);
    }

    public ClientHttpClient Client => _client;
    public MemberHttpClient Member => _member;
    public CustomerHttpClient Customer => _customer;
    public MembershipDueHttpClient MembershipDue => _membershipDue;
    public ReservationHttpClient Reservation => _reservation;
    public DogHttpClient Dog => _dog;
    public InvoiceHttpClient Invoice => _invoice;
    public ItemHttpClient Item => _item;
    public ReportHttpClient Report => _report;
    public ReasonForRejectionHttpClient Reason => _reason;
}
