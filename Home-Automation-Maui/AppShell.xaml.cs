using Home_Automation_Maui.Views;

namespace Home_Automation_Maui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
	}
}
