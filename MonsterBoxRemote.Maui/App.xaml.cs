using Microsoft.Extensions.DependencyInjection;

using MonsterBoxRemote.Maui.Views;

namespace MonsterBoxRemote.Maui;

public partial class App : Application
{
	private readonly IServiceProvider _services;

	public App(IServiceProvider services)
	{
		InitializeComponent();
		_services = services;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		// MainPage is resolved here, lazily, rather than constructor-injected into
		// App - constructor-injecting a page forces DI to build it (parsing its
		// XAML, constructing its handler-backed elements) before the native WinUI3
		// Window/dispatcher exists for it to be threaded into, which reliably
		// fail-fast crashes WinUI3/Windows (0xc000027b in Microsoft.UI.Xaml.dll).
		// Isolated and confirmed with a minimal repro: a stock `dotnet new maui`
		// app crashes identically the moment App(MainPage) constructor injection
		// is added, and stops crashing once MainPage is resolved inside
		// CreateWindow() instead. Don't revert this pattern.
		var mainPage = _services.GetRequiredService<MainPage>();

		// Resolved directly rather than via a resource lookup - ButtonActive/etc.
		// are no longer standalone keyed resources (see AppStyles.xaml for why:
		// AppThemeBinding declared as a resource fail-fast crashes WinUI3/Windows).
		var isDark = Current?.RequestedTheme == AppTheme.Dark;
		var navBarColor = isDark ? Color.FromArgb("#4A1668") : Color.FromArgb("#8929BF");

		var navigationPage = new NavigationPage(mainPage)
		{
			BarTextColor = Colors.White,
			BarBackgroundColor = navBarColor
		};

		return new Window(navigationPage);
	}
}
