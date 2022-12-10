using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Home_Automation_Maui.Models;
using Home_Automation_Maui.Services;
using System.Collections.ObjectModel;

namespace Home_Automation_Maui.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly ISettingService _SettingService;

        [ObservableProperty]
        private ObservableCollection<SettingModel> _Settings;

        public SettingsViewModel(ISettingService settingService) 
        {
            _SettingService = settingService;
            ClearSettings();
        }

        [RelayCommand]
        private void SaveSettings()
        {
            _SettingService.ImportSettings(_Settings);
        }

        [RelayCommand]
        private void ClearSettings()
        {
            _Settings = _SettingService.ExportSettings().ToObservableCollection();
        }
    }
}
