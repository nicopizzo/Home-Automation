using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using Home_Automation_Maui.Services;
using Home_Automation_Maui.ViewModels;
using Home_Automation_Maui.Views;
using Microsoft.Extensions.Logging;

namespace Home_Automation_Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.UseMauiCommunityToolkit()
			.UseMauiCommunityToolkitMarkup();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddAppServices();
		builder.Services.AddViewModels();
        builder.Services.AddViews();

		return builder.Build();
	}
}
