using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
