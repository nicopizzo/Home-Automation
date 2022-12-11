using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using PizzoHomeAutomation.Services;
using PizzoHomeAutomation.ViewModels;
using PizzoHomeAutomation.Views;
using Microsoft.Extensions.Logging;

namespace PizzoHomeAutomation;

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
