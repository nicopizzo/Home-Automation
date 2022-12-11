using Home.Core.Clients.Interfaces;

namespace PizzoHomeAutomation.Services
{
    public interface IGarageService
    {
        Task<int> GetGarageStatus();
        Task ToggleGarage();
    }

    internal sealed class GarageService : IGarageService, IDisposable
    {
        private readonly IGarage _Client;
        private readonly ISettingService _SettingService;
        
        public GarageService(IGarage client, ISettingService settings)
        {
            _Client = client ?? throw new ArgumentNullException(nameof(client));
            _SettingService = settings ?? throw new ArgumentNullException(nameof(settings));
            _Client.BaseAddress = _SettingService.BaseEndpoint;
            _Client.Token = _SettingService.Token;
            _SettingService.SettingChanged += OnSettingsChanged;
        }

        public async Task<int> GetGarageStatus()
        {
            return await _Client.GetGarageStatus();
        }

        public async Task ToggleGarage()
        {
            await _Client.ToggleGarage();
        }

        private void OnSettingsChanged(object sender, Events.SettingChangedEventArgs e)
        {
            switch (e.SettingName)
            {
                case nameof(ISettingService.BaseEndpoint):
                    _Client.BaseAddress = e.SettingValue;
                    break;
                case nameof(ISettingService.Token):
                    _Client.Token = e.SettingValue;
                    break;
            }
        }

        public void Dispose()
        {
            _SettingService.SettingChanged -= OnSettingsChanged;
        }
    }
}
