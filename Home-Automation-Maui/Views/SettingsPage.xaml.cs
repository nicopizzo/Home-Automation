using Home_Automation_Maui.ViewModels;

namespace Home_Automation_Maui.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}