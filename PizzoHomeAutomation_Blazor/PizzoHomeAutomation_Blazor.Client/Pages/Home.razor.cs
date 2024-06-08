using Blazored.LocalStorage;
using Blazored.Toast.Services;
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
    [Inject]
    private IToastService ToastService { get; set; } = default!;

    private string? _GarageStatus = null;
    private Uri _BaseEndpoint = default!;
    private bool _Processing = true;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SetEndpoint();
            await AddTokenHeader();
            await UpdateState();
            StateHasChanged();
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
            else
            {
                ToastService.ShowWarning("Unable to fetch garage status. Please check settings and try again.");
            }
        }
        catch
        {
            _GarageStatus = null;
            ToastService.ShowWarning("Unable to send request. Check settings and network settings then try again");
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
                ToastService.ShowSuccess("Garage Button Pressed!");
                await Task.Delay(5000);
                await UpdateState();
            }
            else
            {
                ToastService.ShowWarning("Unable to press garage button. Please check settings and try again.");
            }
        }
        catch
        {
            ToastService.ShowWarning("Unable to send request. Check settings and network settings then try again");
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

    private async Task SetEndpoint()
    {
        try
        {
            var endpoint = (await LocalStorageService.GetItemAsStringAsync(Settings.ApiUrlName)) ?? Settings.DefaultApiUrl;
            _BaseEndpoint = new Uri(endpoint);
        }
        catch
        {
            ToastService.ShowWarning("Invalid Api Url. Update Settings and try again");
        }
    }
}
