using Blazored.LocalStorage;

namespace SouthSideK9Camp.Client.Endpoints;

public class SouthSideK9CampHttpClient
{

    private readonly ClientHttpClient _client;
    private readonly MemberHttpClient _member;
    private readonly MembershipDueHttpClient _membershipDue;

    public SouthSideK9CampHttpClient(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _client = new(httpClient, localStorage);
        _member = new(httpClient);
        _membershipDue = new(httpClient);
    }

    public ClientHttpClient Client => _client;
    public MemberHttpClient Member => _member;
    public MembershipDueHttpClient MembershipDue => _membershipDue;
}
