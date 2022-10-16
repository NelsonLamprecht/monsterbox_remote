using System.Diagnostics;

using Xamarin.Forms;

using MonsterBoxRemote.Mobile.ViewModel;

namespace MonsterBoxRemote.Mobile.View
{
    public partial class MonsterBoxControllerPage : ContentPage
    {
        private MonsterBoxControllerViewModel ViewModel { get; }

        public MonsterBoxControllerPage()
        {
            InitializeComponent();
            BindingContext = new MonsterBoxControllerViewModel();
            ViewModel = BindingContext as MonsterBoxControllerViewModel;
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