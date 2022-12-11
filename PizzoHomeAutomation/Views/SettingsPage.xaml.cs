using PizzoHomeAutomation.ViewModels;

namespace PizzoHomeAutomation.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}