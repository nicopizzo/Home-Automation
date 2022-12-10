using Home_Automation_Maui.Events;
using Home_Automation_Maui.Models;

namespace Home_Automation_Maui.Services
{
    public interface ISettingService
    {
        event EventHandler<SettingChangedEventArgs> SettingChanged;

        string BaseEndpoint { get; set; }
        string Token { get; set; }

        IEnumerable<SettingModel> ExportSettings();
        void ImportSettings(IEnumerable<SettingModel> settings);
    }

    internal class SettingService : ISettingService
    {
        private readonly IPreferences _Preferences;

        public event EventHandler<SettingChangedEventArgs> SettingChanged;

        public string BaseEndpoint
        {
            get => _Preferences.Get("baseEndpoint", "https://localhost");
            set 
            {
                _Preferences.Set("baseEndpoint", value);
                SettingChanged?.Invoke(this, new SettingChangedEventArgs(nameof(BaseEndpoint), value));
            } 
        }

        public string Token
        {
            get => _Preferences.Get("token", string.Empty);
            set
            {
                _Preferences.Set("token", value);
                SettingChanged?.Invoke(this, new SettingChangedEventArgs(nameof(Token), value));
            }
        }

        public SettingService(IPreferences preferences)
        {
            _Preferences = preferences ?? throw new ArgumentNullException(nameof(preferences));
        }

        public IEnumerable<SettingModel> ExportSettings()
        {
            var settings = new List<SettingModel>();
            var properties = this.GetType().GetProperties();
            foreach (var property in properties)
            {
                settings.Add(new SettingModel() { SettingName = property.Name, SettingValue = property.GetValue(this)?.ToString() });
            }
            return settings;
        }

        public void ImportSettings(IEnumerable<SettingModel> settings)
        {
            var properties = this.GetType().GetProperties();
            foreach (var property in properties)
            {
                var setting = settings.Where(f => f.SettingName == property.Name).FirstOrDefault();
                if (setting != null)
                {
                    property.SetValue(this, setting.SettingValue);
                }
            }
        }
    }
}
