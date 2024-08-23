using Blazored.LocalStorage;

namespace SouthSideK9Camp.Client.Endpoints;

public class SouthSideK9CampHttpClient
{

    private readonly ClientHttpClient _client;
    private readonly MemberHttpClient _member;

    public SouthSideK9CampHttpClient(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _client = new(httpClient, localStorage);
        _member = new(httpClient);
    }

    public ClientHttpClient Client() => _client;
    public MemberHttpClient Member() => _member;
}
