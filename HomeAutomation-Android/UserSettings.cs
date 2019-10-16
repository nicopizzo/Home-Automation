using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace HomeAutomation_Android
{
    public static class UserSettings
    {
        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static string Token
        {
            get => AppSettings.GetValueOrDefault(nameof(Token), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Token), value);
        }

        public static string GarageApiEndpoint
        {
            get => AppSettings.GetValueOrDefault(nameof(GarageApiEndpoint), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(GarageApiEndpoint), value);
        }

        public static void ClearAllData()
        {
            AppSettings.Clear();
        }

    }
}