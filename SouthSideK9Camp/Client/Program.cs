using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SouthSideK9Camp.Client;
using SouthSideK9Camp.Client.Endpoints;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("SouthSideK9Camp.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("SouthSideK9Camp.ServerAPI"));

// Mudblazor Services
builder.Services.AddMudServices();

// Blazored Services
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();

// SouthSideK9Camp HttpClient Service
builder.Services.AddScoped<SouthSideK9CampHttpClient>();

await builder.Build().RunAsync();
