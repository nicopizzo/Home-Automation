using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PizzoHomeAutomation.Models;
using PizzoHomeAutomation.Services;
using System.Collections.ObjectModel;

namespace PizzoHomeAutomation.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly ISettingService _SettingService;
        private readonly IToastService _ToastService;

        [ObservableProperty]
        private ObservableCollection<SettingModel> _Settings;

        public SettingsViewModel(ISettingService settingService, IToastService toastService) 
        {
            _SettingService = settingService;
            _ToastService = toastService;
            Settings = _SettingService.ExportSettings().ToObservableCollection();
        }

        [RelayCommand]
        private async Task SaveSettings()
        {
            _SettingService.ImportSettings(_Settings);
            await _ToastService.Show("Settings updated");
        }

        [RelayCommand]
        private async Task ClearSettings()
        {
            Settings = _SettingService.ExportSettings().ToObservableCollection();
            await _ToastService.Show("Settings updated");
        }
    }
}
