using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace PizzoHomeAutomation_Blazor.Client.Pages;

public partial class Settings
{
    [Inject]
    private ILocalStorageService LocalStorageService { get; set; } = default!;

    private string _ApiKey = string.Empty;
    public static readonly string ApiKeyName = "apiKey";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _ApiKey = (await LocalStorageService.GetItemAsStringAsync(ApiKeyName)) ?? string.Empty;
            StateHasChanged();
        }
    }

    private async Task OnSaveClick()
    {
        await LocalStorageService.SetItemAsStringAsync(ApiKeyName, _ApiKey);
    }
}
