using MobileMaple.ViewModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace MobileMaple.View
{
    public partial class LedControllerPage : ContentPage
    {
        public LedControllerPage()
        {
            InitializeComponent();
            BindingContext = new LedControllerViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await (BindingContext as LedControllerViewModel).GetServers();
        }

        private void beginIterationSlider_ValueChanged(object sender, ValueChangedEventArgs args)
        {
            double value = args.NewValue;
            Debug.WriteLine(value);
        }
    }
}