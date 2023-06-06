using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Media;
using MagicConchShellMaui.Views;
using Microsoft.Extensions.Logging;

namespace MagicConchShellMaui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMediaElement()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddSingleton(SpeechToText.Default);
        builder.Services.AddSingleton<MainPage, MainPageView>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
