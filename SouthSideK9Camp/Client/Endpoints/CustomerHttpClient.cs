using Blazored.LocalStorage;
using System.Net;

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
}
