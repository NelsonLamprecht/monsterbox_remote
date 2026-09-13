using Microsoft.Maui.Handlers;

namespace MonsterBoxRemote.Maui;

public static partial class MauiProgram
{
	static partial void RegisterPlatformHandlers(MauiAppBuilder builder)
	{
		builder.ConfigureMauiHandlers(handlers =>
		{
			// Strips the default Android Picker chrome (background/margins/padding)
			// so it matches the app's flat, custom-colored button styling. iOS
			// intentionally does not get an equivalent handler - see agents.md /
			// the UI design review notes on respecting the native wheel picker.
			PickerHandler.Mapper.AppendToMapping("StylelessPicker", (handler, view) =>
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
}
