using PizzoHomeAutomation.Views;

namespace PizzoHomeAutomation;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
	}
}
