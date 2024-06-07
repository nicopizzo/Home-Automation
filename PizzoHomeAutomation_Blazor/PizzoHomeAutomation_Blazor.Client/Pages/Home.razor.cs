using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using PizzoHomeAutomation_Blazor.Client.Models;
using System.Net.Http.Json;

namespace PizzoHomeAutomation_Blazor.Client.Pages;

public partial class Home
{
    [Inject]
    private ILocalStorageService LocalStorageService { get; set; } = default!;
    [Inject]
    private HttpClient HttpClient { get; set; } = default!;

    private string? _GarageStatus = null;
    private Uri _BaseEndpoint = new Uri("http://garage.local:8000/");
    private bool _Processing = true;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await AddTokenHeader();
            await UpdateState();
        }
    }

    private async Task UpdateState()
    {
        try
        {
            _GarageStatus = null;
            var uri = new Uri(_BaseEndpoint, "/api/Garage/getGarageStatus");
            var response = await HttpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var status = await response.Content.ReadFromJsonAsync<GarageStatus>();
                if (status == GarageStatus.Open)
                {
                    _GarageStatus = "Open";
                }
                else
                {
                    _GarageStatus = "Closed";
                }
            }
        }
        catch
        {
            _GarageStatus = null;
        }
        _Processing = false;
    }

    private async Task ToggleState()
    {
        try
        {
            _Processing = true;
            var uri = new Uri(_BaseEndpoint, "/api/Garage/toggleGarage");
            var response = await HttpClient.PutAsync(uri, null);
            if (response.IsSuccessStatusCode)
            {
                await Task.Delay(5000);
                await UpdateState();
            }
        }
        catch
        {
        }
    }

    private async Task AddTokenHeader()
    {
        var apiKey = (await LocalStorageService.GetItemAsStringAsync(Settings.ApiKeyName)) ?? string.Empty;
        if (HttpClient.DefaultRequestHeaders.Contains("Token"))
        {
            HttpClient.DefaultRequestHeaders.Remove("Token");
        }
        HttpClient.DefaultRequestHeaders.Add("Token", apiKey);
    }
}
