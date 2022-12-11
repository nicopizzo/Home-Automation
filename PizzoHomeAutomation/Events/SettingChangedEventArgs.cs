namespace PizzoHomeAutomation.Events
{
    public class SettingChangedEventArgs : EventArgs
    {
        public string SettingName { get; set; }
        public string SettingValue { get; set; }

        public SettingChangedEventArgs(string settingName, string value) 
        {
            SettingName = settingName;
            SettingValue = value;
        }
    }
}
