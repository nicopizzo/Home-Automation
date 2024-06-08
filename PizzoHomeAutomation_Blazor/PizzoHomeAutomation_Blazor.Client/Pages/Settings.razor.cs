using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;

namespace PizzoHomeAutomation_Blazor.Client.Pages;

public partial class Settings
{
    [Inject]
    private ILocalStorageService LocalStorageService { get; set; } = default!;
    [Inject]
    private IToastService ToastService { get; set; } = default!;

    private string _ApiKey = string.Empty;
    private string _ApiUrl = string.Empty;
    public static readonly string DefaultApiUrl = "https://garage.local:8000";
    public static readonly string ApiKeyName = "apiKey";
    public static readonly string ApiUrlName = "apiUrl";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _ApiKey = (await LocalStorageService.GetItemAsStringAsync(ApiKeyName)) ?? string.Empty;
            _ApiUrl = (await LocalStorageService.GetItemAsStringAsync(ApiUrlName)) ?? DefaultApiUrl;
            StateHasChanged();
        }
    }

    private async Task OnSaveClick()
    {
        await LocalStorageService.SetItemAsStringAsync(ApiKeyName, _ApiKey);
        await LocalStorageService.SetItemAsStringAsync(ApiUrlName, _ApiUrl);
        ToastService.ShowSuccess("Settings Saved!");
    }
}
