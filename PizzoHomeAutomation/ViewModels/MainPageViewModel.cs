using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PizzoHomeAutomation.Services;
using PizzoHomeAutomation.Views;

namespace PizzoHomeAutomation.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly IGarageService _GarageService;
        private readonly IToastService _ToastService;

        [ObservableProperty]
        private string _GarageStatus = "Unknown";

        public MainPageViewModel(IGarageService garageService, IToastService toastService)
        {
            _GarageService = garageService ?? throw new ArgumentNullException(nameof(garageService));
            _ToastService = toastService ?? throw new ArgumentNullException(nameof(toastService));
        }

        public async Task OnAppearing()
        { 
            await RefreshGarageStatus();
        }

        [RelayCommand]
        private async Task ToggleGarage()
        {
            try
            {
                await _GarageService.ToggleGarage();
                await _ToastService.Show("Garage Button Pushed");
            }
            catch
            {
                await _ToastService.Show("Failed to push button");
            }           
        }

        [RelayCommand]
        private async Task RefreshGarageStatus()
        {
            try
            {
                var result = await _GarageService.GetGarageStatus();
                GarageStatus = result == 0 ? "Closed" : "Open";
            }
            catch
            {
                GarageStatus = "Unknown";
                await _ToastService.Show("Failed get latest status");
            }
        }

        [RelayCommand]
        private async Task NavigateToSettings()
        {
            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }
    }
}
