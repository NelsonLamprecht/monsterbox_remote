using Microsoft.Extensions.Logging;

using MonsterBoxRemote.Maui.ViewModel;
using MonsterBoxRemote.Maui.Views;

namespace MonsterBoxRemote.Maui;

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
			});

#if ANDROID
		RegisterAndroidHandlers(builder);
#endif

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

#if ANDROID
	private static void RegisterAndroidHandlers(MauiAppBuilder builder)
	{
		builder.ConfigureMauiHandlers(handlers =>
		{
			Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("StylelessPicker", (handler, view) =>
			{
				var control = handler.PlatformView;
				control.Background = null;

				var layoutParams = new Android.Views.ViewGroup.MarginLayoutParams(control.LayoutParameters);
				layoutParams.SetMargins(0, 0, 0, 0);
				control.LayoutParameters = layoutParams;
				control.SetPadding(0, 0, 0, 0);
			});
		});
	}
#endif
}
