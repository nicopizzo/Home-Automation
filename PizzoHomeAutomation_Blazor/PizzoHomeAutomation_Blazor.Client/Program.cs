using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PizzoHomeAutomation_Blazor.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddBlazorClientServices();

await builder.Build().RunAsync();
