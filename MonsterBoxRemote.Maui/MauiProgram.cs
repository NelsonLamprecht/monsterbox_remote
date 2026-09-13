using Microsoft.Extensions.Logging;

using MonsterBoxRemote.Maui.ViewModel;
using MonsterBoxRemote.Maui.Views;

namespace MonsterBoxRemote.Maui;

public static partial class MauiProgram
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
			});

		// Implemented once per Platforms/<X>/ folder (see MauiProgram.Android.cs);
		// a no-op on platforms that don't need a handler customization.
		RegisterPlatformHandlers(builder);

		// Shared view model instance across Controller/Options pages so selected
		// devices and stepper values survive navigation between the two pages.
		builder.Services.AddSingleton<MonsterBoxControllerViewModel>();

		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<MonsterBoxControllerPage>();
		builder.Services.AddTransient<MonsterBoxOptionsPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	static partial void RegisterPlatformHandlers(MauiAppBuilder builder);
}
