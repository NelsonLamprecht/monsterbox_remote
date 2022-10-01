using System.Diagnostics;

using Xamarin.Forms;

using MonsterBoxRemote.Mobile.ViewModel;

namespace MonsterBoxRemote.Mobile.View
{
    public partial class LedControllerPage : ContentPage
    {
        private LedControllerViewModel ViewModel { get; }

        public LedControllerPage()
        {
            InitializeComponent();
            BindingContext = new LedControllerViewModel();
            ViewModel = BindingContext as LedControllerViewModel;
        }        

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.GetServers();
        }
    

        private void BeginIterationsStepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Debug.WriteLine(e.NewValue);
            if (int.TryParse(e.NewValue.ToString(), out var value))
            {
                ViewModel.BeginIterations = value;
            }
        }

        private void EndIterationsStepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Debug.WriteLine(e.NewValue);
            if (int.TryParse(e.NewValue.ToString(), out var value))
            {                
                ViewModel.EndIterations = value;
            }
        }

        private void BeginDelayStepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Debug.WriteLine(e.NewValue);
            if (int.TryParse(e.NewValue.ToString(), out var value))
            {
                ViewModel.BeginDelay = value;
            }
        }

        private void EndDelayStepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Debug.WriteLine(e.NewValue);
            if (int.TryParse(e.NewValue.ToString(), out var value))
            {
                ViewModel.EndDelay = value;
            }
        }
    }
}