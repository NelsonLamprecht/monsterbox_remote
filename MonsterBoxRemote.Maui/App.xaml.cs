using MonsterBoxRemote.Maui.Views;

namespace MonsterBoxRemote.Maui;

public partial class App : Application
{
	private readonly MainPage _mainPage;

	public App(MainPage mainPage)
	{
		InitializeComponent();
		_mainPage = mainPage;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var navigationPage = new NavigationPage(_mainPage)
		{
			BarTextColor = Colors.White,
			BarBackgroundColor = (Color)Current!.Resources["ButtonActive"]
		};

		return new Window(navigationPage);
	}
}
