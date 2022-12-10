using Home.Core.Clients.Interfaces;

namespace Home_Automation_Maui.Services
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

        private string _CurrentEndpoint;
        private string _CurrentToken;
        
        public GarageService(IGarage client, ISettingService settings)
        {
            _Client = client ?? throw new ArgumentNullException(nameof(client));
            _SettingService = settings ?? throw new ArgumentNullException(nameof(settings));
            _CurrentEndpoint = _SettingService.BaseEndpoint;
            _CurrentToken = _SettingService.Token;
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
            var changed = false;
            switch (e.SettingName)
            {
                case nameof(ISettingService.BaseEndpoint):
                    _CurrentEndpoint = e.SettingValue;
                    changed = true;
                    break;
                case nameof(ISettingService.Token):
                    _CurrentToken = e.SettingValue;
                    changed = true;
                    break;
            }

            if (changed)
            {
                _Client.UpdateBaseAndToken(_CurrentEndpoint, _CurrentToken);
            }
        }

        public void Dispose()
        {
            _SettingService.SettingChanged -= OnSettingsChanged;
        }
    }
}
